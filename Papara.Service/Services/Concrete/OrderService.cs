using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Extensions;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using Papara.Service.Utilities;
using System.Linq.Expressions;
using System.Text;

namespace Papara.Service.Services.Concrete
{
	public class OrderService : IOrderService
	{
		private readonly IGenericRepository<Order> _repository;
		private readonly IBasketItemService _basketItemService;
		private readonly IBasketService _basketService;
		private readonly IProductService _productService;
		private readonly IDigitalWalletService _digitalWalletService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IEmailService _emailService;
		private readonly RabbitMQPublisher _rabbitMQPublisher;


		public OrderService(IGenericRepository<Order> repository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IBasketService basketService, IDigitalWalletService digitalWalletService, IBasketItemService basketItemService, IProductService productService, IEmailService emailService, RabbitMQPublisher rabbitMQPublisher)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_basketService = basketService;
			_unitOfWork = unitOfWork;
			_digitalWalletService = digitalWalletService;
			_basketItemService = basketItemService;
			_productService = productService;
			_emailService = emailService;
			_rabbitMQPublisher = rabbitMQPublisher;
		}

		public async Task<CustomResponseDto<OrderWithDetailResponseDTO?>> GetOrderWithDetailAsync(Expression<Func<Order, bool>> predicate, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false)
		{
			var order = await _repository.GetAsync(
				predicate,
				include: o => o.Include(u => u.User)
							   .Include(od => od.OrderDetails)
							   .ThenInclude(p => p.Product),
				withDeleted);

			if (order == null)
				return CustomResponseDto<OrderWithDetailResponseDTO?>.Fail(404, "Order not found");

			var dto = _mapper.Map<OrderWithDetailResponseDTO>(order);
			return CustomResponseDto<OrderWithDetailResponseDTO?>.Success(200, dto);
		}

		public async Task<CustomResponseDto<List<OrderWithDetailResponseDTO>>> GetAllOrdersWithDetailAsync(bool withDeleted = false)
		{
			var orders = await _repository.GetListAsync(
					include: o => o.Include(u => u.User)
								   .Include(od => od.OrderDetails)
								   .ThenInclude(p => p.Product),
					withDeleted: withDeleted
				);

			var dtos = _mapper.Map<List<OrderWithDetailResponseDTO>>(orders);
			return CustomResponseDto<List<OrderWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<List<OrderWithDetailResponseDTO>>> GetMyOrders()
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();

			var orders = await _repository.GetListAsync(
				predicate: o => o.UserId == userId,
				include: o => o.Include(u => u.User)
							   .Include(od => od.OrderDetails)
							   .ThenInclude(p => p.Product),
				withDeleted: true);

			if (orders == null || !orders.Any())
				return CustomResponseDto<List<OrderWithDetailResponseDTO>>.Fail(404, Messages.OrderNotFound);

			var orderedOrders = orders.OrderByDescending(o => o.CreatedDate).ToList(); // en yeniden eskiye sıralama 
			var dtos = _mapper.Map<List<OrderWithDetailResponseDTO>>(orderedOrders);

			return CustomResponseDto<List<OrderWithDetailResponseDTO>>.Success(200, dtos);
		}
		public async Task<CustomResponseDto<OrderResponseDTO>> CreateOrder()
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();

			var basket = await _basketService.GetBasketWithDetailAsync(b => b.UserId == userId && b.IsActive,
				include: b => b.Include(i => i.Items).ThenInclude(p => p.Product));

			if (basket == null || !basket.Data.Items.Any())
				return CustomResponseDto<OrderResponseDTO>.Fail(400, Messages.NoActiveBasketOrBasketIsEmpty);

			foreach (var item in basket.Data.Items)
			{
				var product = await _productService.GetAsync(p => p.Id == item.ProductId);
				if (product.Data.Stock < item.Quantity)
					return CustomResponseDto<OrderResponseDTO>.Fail(400, "Not enough stock for product: " + product.Data.Name);

				// Stoktan düş
				product.Data.Stock -= item.Quantity;
				await _productService.UpdateStockAsync(product.Data.Id, _mapper.Map<ProductRequestDTO>(product.Data));
			}

			var totalAmount = basket.Data.FinalPrice;
			decimal userPointsToApply = 0;

			var wallet = await _digitalWalletService.GetAsync(w => w.UserId == userId);
			if (wallet != null && wallet.Points > 0)
			{
				userPointsToApply = wallet.Points;
				totalAmount -= userPointsToApply;  // Puanlar toplam tutardan düşülüyor
			}

			var order = new Order
			{
				UserId = userId,
				OrderNumber = GenerateOrderNumber(),  // Sipariş numarası oluştur
				TotalAmount = totalAmount,     // Sepetteki final fiyat
				PointUsed = userPointsToApply,  // Kullanılan puan
				OrderDetails = basket.Data.Items.Select(i => new OrderDetail
				{
					ProductId = i.ProductId,
					Quantity = i.Quantity,
					Price = i.Price,
					CreatedDate = i.CreatedDate
				}).ToList()
			};

			// Cüzdan, puan kontrolü ve düşürme işlemi
			var walletResponse = await _digitalWalletService.ReduceWalletBalance(userId, totalAmount, userPointsToApply);
			if (!walletResponse.IsSuccessful)
				return CustomResponseDto<OrderResponseDTO>.Fail(walletResponse.StatusCode, walletResponse.Errors.First());

			await _repository.AddAsync(order);
			await _unitOfWork.CompleteAsync();

			// Kazanılan puanları hesapla ve cüzdana ekle
			decimal totalPointsEarned = basket.Data.Items.Sum(i => CalculatePoints(i.Price, i.Quantity, i.PointsPercentage, i.MaxPoint));
			wallet.Points += totalPointsEarned;

			var walletUpdateDto = new DigitalWalletRequestDTO
			{
				UserId = wallet.UserId,
				Points = wallet.Points,
				Balance = wallet.Balance
			};

			await _digitalWalletService.UpdateAsync(wallet.Id, walletUpdateDto);

			await UpdateBasketAndItemsStatus(basket.Data.Id);

			var newBasket = new Basket { UserId = userId };
			await _basketService.AddBasketAsync(newBasket);
			await _unitOfWork.CompleteWithTransaction();

			var orderDto = _mapper.Map<OrderResponseDTO>(order);

			SendEmail(order.User.Email, $"{order.User.FirstName} {order.User.LastName}", orderDto);

			return CustomResponseDto<OrderResponseDTO>.Success(200, orderDto);
		}

		public void SendEmail(string email, string name, OrderResponseDTO orderDto)
		{

			var htmlContent = $@"
					<html>
					<head>
						<style>
							table {{
								width: 100%;
								border-collapse: collapse;
							}}
							table, th, td {{
								border: 1px solid black;
							}}
							th, td {{
								padding: 8px;
								text-align: left;
							}}
						</style>
					</head>
					<body>
						<h1>Sipariş Onayı</h1>
						<p>Merhaba {name},</p>
						<p>Sipariş Numaranız: <strong>{orderDto.OrderNumber}</strong></p>
						<p>Toplam Tutar: <strong>{orderDto.TotalAmount} TL</strong></p>
						<p>Kullanılan Puan: <strong>{orderDto.PointUsed}</strong></p>
						<p>Sipariş Tarihi: <strong>{DateTime.Now:yyyy-MM-dd}</strong></p>
					</body>
					</html>";

			var message = JsonConvert.SerializeObject(new
			{
				Email = email,
				Name = name,
				Subject = "Sipariş Onayı",
				Content = htmlContent
			});

			_rabbitMQPublisher.Publish(message);
		}

	
		private async Task UpdateBasketAndItemsStatus(int basketId)
		{
			var basket = await _basketService.GetBasketAsync(x => x.Id == basketId);
			if (basket != null)
			{
				await _basketService.SoftDeleteAsync(basket.Data.Id);

				foreach (var item in basket.Data.Items)
				{
					await _basketItemService.SoftDeleteBasketItemAsync(item.Id);
				}
				await _unitOfWork.CompleteWithTransaction();
			}
		}

		private string GenerateOrderNumber()
		{
			// Sipariş numarası üretmek için
			return new Random().Next(100000000, 999999999).ToString();
		}

		private decimal CalculatePoints(decimal price, int quantity, decimal pointsPercentage, decimal maxPoint)
		{
			decimal points = price * quantity * pointsPercentage / 100;
			return points > maxPoint ? maxPoint : points;
		}

		public async Task<CustomResponseDto<OrderResponseDTO>> GetAsync(Expression<Func<Order, bool>> predicate, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false)
		{
			var order = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(order);

			var orderDto = _mapper.Map<OrderResponseDTO>(order);
			return CustomResponseDto<OrderResponseDTO>.Success(200, orderDto);
		}

		public async Task<CustomResponseDto<List<OrderResponseDTO>>> GetListAsync(Expression<Func<Order, bool>>? predicate = null,
			Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false)
		{
			List<Order> orders = await _repository.GetListAsync(withDeleted: false);
			var ordersDto = _mapper.Map<List<OrderResponseDTO>>(orders);
			return CustomResponseDto<List<OrderResponseDTO>>.Success(200, ordersDto);
		}

		public async Task<bool> AnyAsync(Expression<Func<Order, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}

		public async Task<CustomResponseDto<OrderResponseDTO>> AddAsync(OrderRequestDTO orderRequestDTO)
		{
			var order = _mapper.Map<Order>(orderRequestDTO);

			var addedOrder = await _repository.AddAsync(order);

			var orderResponseDTO = _mapper.Map<OrderResponseDTO>(addedOrder);
			return CustomResponseDto<OrderResponseDTO>.Success(201, orderResponseDTO);
		}

		public async Task<CustomResponseDto<OrderResponseDTO>> UpdateAsync(int id, OrderRequestDTO orderRequestDTO)
		{

			var existingOrder = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingOrder);
			_mapper.Map(orderRequestDTO, existingOrder);

			var updatedOrder = await _repository.UpdateAsync(existingOrder);
			await _unitOfWork.CompleteAsync();

			var updatedOrderDto = _mapper.Map<OrderResponseDTO>(updatedOrder);
			return CustomResponseDto<OrderResponseDTO>.Success(200, updatedOrderDto);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			await _repository.HardDeleteAsync(id);
			return CustomResponseDto<bool>.Success(204, true);
		}


		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			await _repository.SoftDeleteAsync(id);

			return CustomResponseDto<bool>.Success(204, true);
		}
	}


}

