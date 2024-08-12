using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Extensions;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class BasketItemService : IBasketItemService
	{
		private readonly IGenericRepository<BasketItem> _repository;
		private readonly IProductService _productService;
		private readonly IBasketService _basketService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BasketItemService(IGenericRepository<BasketItem> repository, IMapper mapper, IUnitOfWork unitOfWork, IBasketService basketService, IProductService productService, IHttpContextAccessor httpContextAccessor)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_basketService = basketService;
			_productService = productService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<CustomResponseDto<List<BasketItemWithDetailResponseDTO>>> GetAllBasketItemsWithDetailAsync(bool withDeleted = false)
		{
			var basketItems = await _repository.GetListAsync(
			   include: bi => bi.Include(x => x.Product)
								.Include(x => x.Basket).ThenInclude(b => b.User), withDeleted: withDeleted);

			var dtos = _mapper.Map<List<BasketItemWithDetailResponseDTO>>(basketItems);
			return CustomResponseDto<List<BasketItemWithDetailResponseDTO>>.Success(200, dtos);
		}
		public async Task<CustomResponseDto<BasketItemWithDetailResponseDTO?>> GetBasketItemWithDetailAsync(Expression<Func<BasketItem, bool>> predicate, Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false)
		{
			var basketItem = await _repository.GetAsync(
			  predicate,
			  bi => bi.Include(x => x.Product)
					  .Include(x => x.Basket).ThenInclude(b => b.User),
			  withDeleted);

			if (basketItem == null)
			{
				return CustomResponseDto<BasketItemWithDetailResponseDTO?>.Fail(404, Messages.BasketItemNotFound);
			}

			var dto = _mapper.Map<BasketItemWithDetailResponseDTO>(basketItem);
			return CustomResponseDto<BasketItemWithDetailResponseDTO?>.Success(200, dto);
		}
		public async Task<CustomResponseDto<BasketItemResponseDTO>> UpdateBasketItemAsync(int id, BasketItemRequestDTO basketItemRequest)
		{

			var existingItem = await _repository.GetAsync(bi => bi.Id == id);
			if (existingItem == null)
				return CustomResponseDto<BasketItemResponseDTO>.Fail(404, Messages.BasketItemNotFound);

			var product = await _productService.GetAsync(x => x.Id == existingItem.ProductId);
			if (product == null)
				return CustomResponseDto<BasketItemResponseDTO>.Fail(404, Messages.ProductNotFound);

			if (basketItemRequest.Quantity > existingItem.Quantity)
			{
				int additionalQuantity = basketItemRequest.Quantity;
				if (product.Data.Stock < additionalQuantity)
					return CustomResponseDto<BasketItemResponseDTO>.Fail(409, Messages.NotEnoughStockAvailable);
				product.Data.Stock -= additionalQuantity;
			}
			else
			{
				int returnedQuantity = existingItem.Quantity - basketItemRequest.Quantity;
				product.Data.Stock -= returnedQuantity;
			}


			_mapper.Map(basketItemRequest, existingItem);
			await _repository.UpdateAsync(existingItem);
			await _unitOfWork.CompleteAsync();
			var responseDto = _mapper.Map<BasketItemResponseDTO>(existingItem);
			return CustomResponseDto<BasketItemResponseDTO>.Success(200, responseDto);


		}

		public async Task<CustomResponseDto<bool>> SoftDeleteBasketItemAsync(int id)
		{
			var existingItem = await _repository.GetAsync(bi => bi.Id == id);
			if (existingItem == null)
				return CustomResponseDto<bool>.Fail(404, Messages.BasketItemNotFound);

			var product = await _productService.GetAsync(x => x.Id == existingItem.ProductId);
			if (product != null)
			{
				await _repository.SoftDeleteAsync(existingItem.Id);
				await _unitOfWork.CompleteAsync();
			}

			return CustomResponseDto<bool>.Success(204);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteBasketItemAsync(int id)
		{
			await _repository.HardDeleteAsync(id);
			try
			{
				await _unitOfWork.CompleteWithTransaction();
			}
			catch (DbUpdateException)
			{
				throw new DeleteRestrictionException(Messages.CannotDeleteRecordDueToRelatedData);

			}
			return CustomResponseDto<bool>.Success(204);
		}

		public async Task<CustomResponseDto<BasketItemResponseDTO>> AddItemToBasketAsync(BasketItemRequestDTO basketItemRequest)
		{
			var basket = await _basketService.GetBasketAsync(x => x.Id == basketItemRequest.BasketId);
			if (basket.Data == null)
				return CustomResponseDto<BasketItemResponseDTO>.Fail(404, Messages.BasketNotFound);

			var product = await _productService.GetAsync(x => x.Id == basketItemRequest.ProductId);
			if (product == null)
				return CustomResponseDto<BasketItemResponseDTO>.Fail(404, Messages.ProductNotFound);

			if (product.Data.Stock < basketItemRequest.Quantity)
				return CustomResponseDto<BasketItemResponseDTO>.Fail(409, Messages.NotEnoughStockAvailable);

			var newItem = _mapper.Map<BasketItem>(basketItemRequest);
			newItem.PointsEarned = CalculatePoints(product.Data.Price, basketItemRequest.Quantity, product.Data.PointsPercentage, product.Data.MaxPoint);
			await _repository.AddAsync(newItem);

			await _unitOfWork.CompleteAsync();
			var responseDto = _mapper.Map<BasketItemResponseDTO>(await _repository.GetAsync(bi => bi.BasketId == basketItemRequest.BasketId && bi.ProductId == basketItemRequest.ProductId));
			return CustomResponseDto<BasketItemResponseDTO>.Success(200, responseDto);
		}

		/// <summary>
		/// Üründen kazanılacak puanları hesaplar.
		/// </summary>
		/// <param name="price">Ürünün birim fiyatı.</param>
		/// <param name="quantity">Satın alınan ürün miktarı.</param>
		/// <param name="pointsPercentage">Puan kazanım yüzdesi.</param>
		/// <param name="maxPoint">Üründen kazanılabilecek maksimum puan.</param>
		/// <returns>Kazanılan toplam puan.</returns>
		public decimal CalculatePoints(decimal price, int quantity, decimal pointsPercentage, decimal maxPoint)
		{
			decimal totalCost = price * quantity;
			decimal calculatedPoints = totalCost * (pointsPercentage / 100);
			return Math.Min(calculatedPoints, maxPoint);
		}

		public async Task<CustomResponseDto<BasketItemResponseDTO>> GetAsync(Expression<Func<BasketItem, bool>> predicate, Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false)
		{
			var basketItem = await _repository.GetAsync(predicate);
			BusinessRules.CheckEntityExists(basketItem);

			var basketItemDto = _mapper.Map<BasketItemResponseDTO>(basketItem);
			return CustomResponseDto<BasketItemResponseDTO>.Success(200, basketItemDto); ;
		}

		public async Task<CustomResponseDto<List<BasketItemResponseDTO>>> GetListAsync(Expression<Func<BasketItem, bool>>? predicate = null, Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false)
		{
			List<BasketItem> basketItems = await _repository.GetListAsync(withDeleted: false);
			var basketItemsDto = _mapper.Map<List<BasketItemResponseDTO>>(basketItems);
			return CustomResponseDto<List<BasketItemResponseDTO>>.Success(200, basketItemsDto);
		}
	}
}

