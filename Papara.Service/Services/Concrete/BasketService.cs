using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class BasketService : IBasketService
	{
		private readonly IGenericRepository<Basket> _repository;
		private readonly IDigitalWalletService _walletService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ICouponService _couponService;
		private readonly ICouponUsageService _couponUsageService;
		private readonly BasketBusinessRules _basketBusinessRules;


		public BasketService(IGenericRepository<Basket> repository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ICouponService couponService, IDigitalWalletService walletService, ICouponUsageService couponUsageService, BasketBusinessRules basketBusinessRules)
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
			_couponService = couponService;
			_walletService = walletService;
			_couponUsageService = couponUsageService;
			_basketBusinessRules = basketBusinessRules;
		}

		public async Task<CustomResponseDto<BasketWithDetailResponseDTO?>> GetBasketWithDetailAsync(Expression<Func<Basket, bool>> predicate, Func<IQueryable<Basket>,
			IIncludableQueryable<Basket, object>>? include = null, bool withDeleted = false)
		{
			var basket = await _repository.GetAsync(
				predicate,
				include: b => b.Include(u => u.User)
							   .Include(bi => bi.Items.Where(i => i.IsActive))
							   .ThenInclude(i => i.Product),
				withDeleted: withDeleted);

			if (basket == null)
				return CustomResponseDto<BasketWithDetailResponseDTO?>.Fail(404, Messages.BasketNotFound);

			basket.TotalPrice = CalculateTotalPrice(basket);

			var dto = _mapper.Map<BasketWithDetailResponseDTO>(basket);
			return CustomResponseDto<BasketWithDetailResponseDTO?>.Success(200, dto);
		}

		

		public async Task<CustomResponseDto<BasketResponseDTO?>> GetBasketAsync(Expression<Func<Basket, bool>> predicate, Func<IQueryable<Basket>,
			IIncludableQueryable<Basket, object>>? include = null, bool withDeleted = false)
		{
			var basket = await _repository.GetAsync(
				predicate,
				withDeleted: withDeleted);

			if (basket == null)
				return CustomResponseDto<BasketResponseDTO?>.Fail(404, Messages.BasketNotFound);

			basket.TotalPrice = CalculateTotalPrice(basket);

			var dto = _mapper.Map<BasketResponseDTO>(basket);
			return CustomResponseDto<BasketResponseDTO?>.Success(200, dto);
		}

		public async Task<CustomResponseDto<List<BasketWithDetailResponseDTO>>> GetAllBasketsWithDetailAsync(bool withDeleted = false)
		{
			var baskets = await _repository.GetListAsync(
				include: b => b.Include(u => u.User).Include(bi => bi.Items).ThenInclude(i => i.Product),
				withDeleted: withDeleted);

			foreach (var basket in baskets)
			{
				basket.TotalPrice = CalculateTotalPrice(basket);

			}

			var dtos = _mapper.Map<List<BasketWithDetailResponseDTO>>(baskets);
			return CustomResponseDto<List<BasketWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<List<BasketResponseDTO>>> GetAllBasketsAsync(bool withDeleted = false)
		{
			var baskets = await _repository.GetListAsync(withDeleted: withDeleted);

			var dtos = _mapper.Map<List<BasketResponseDTO>>(baskets);
			return CustomResponseDto<List<BasketResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<BasketWithDetailResponseDTO>> GetCurrentUserBasket()
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();
			var basket = await GetActiveUserBasketAsync(userId);

			if (basket == null)
				return CustomResponseDto<BasketWithDetailResponseDTO>.Fail(404, Messages.BasketNotFound);

			basket.TotalPrice = CalculateTotalPrice(basket);

			var dto = _mapper.Map<BasketWithDetailResponseDTO>(basket);
			return CustomResponseDto<BasketWithDetailResponseDTO>.Success(200, dto);
		}

		private async Task<Basket> GetActiveUserBasketAsync(string userId)
		{
			return await _repository.GetAsync(
				predicate: b => b.UserId == userId && b.IsActive,
				include: b => b.Include(u => u.User)
							   .Include(bi => bi.Items)
							   .ThenInclude(i => i.Product),
				withDeleted: false);
		}

		public decimal CalculateTotalPrice(Basket basket)
		{
			return basket.Items.Sum(item => item.Quantity * item.Product.Price);
		}

		public async Task<CustomResponseDto<BasketWithDetailResponseDTO>> CalculateBasketItemsPriceAsync(BasketRequestDTO basketRequestDTO)
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();
			var basket = await GetActiveUserBasketAsync(userId);

			if (basket == null)
			{
				basket = new Basket { UserId = userId };
				await _repository.AddAsync(basket);
				await _unitOfWork.CompleteAsync();
			}

			decimal totalPointsEarned = 0;
			foreach (var item in basket.Items)
			{
				decimal points = CalculatePoints(item.Quantity * item.Product.Price, item.Product.PointsPercentage, item.Product.MaxPoint);
				item.PointsEarned = points;
				totalPointsEarned += points;
			}

			basket.PointsEarned = totalPointsEarned;
			basket.TotalPrice = CalculateTotalPrice(basket);


			if (!string.IsNullOrWhiteSpace(basketRequestDTO.CouponCode))
			{
				var couponResponse = await HandleCouponCodeAsync(basketRequestDTO.CouponCode, basket, userId);
				if (!couponResponse.IsSuccessful)
					return CustomResponseDto<BasketWithDetailResponseDTO>.Fail(couponResponse.StatusCode, couponResponse.Errors);

				basket.DiscountAmount = couponResponse.Data;
			}


			var wallet = await _walletService.GetAsync(w => w.UserId == userId);
			if (wallet == null)
			{
				wallet = new DigitalWallet { UserId = userId, Points = 0, Balance = 0 };
				await _walletService.AddAsync(wallet);
			}

			basket.FinalPrice = basket.TotalPrice - (basket.DiscountAmount ?? 0m);

			await _unitOfWork.CompleteAsync();
			var response = _mapper.Map<BasketWithDetailResponseDTO>(basket);
			return CustomResponseDto<BasketWithDetailResponseDTO>.Success(200, response);
		}

	

		public async Task<CustomResponseDto<BasketResponseDTO>> EmptyBasket(string userId)
		{
			var existingBasket = await _repository.GetAsync(b => b.UserId == userId && b.IsActive);
			if (existingBasket == null)
			{
				var basket = new Basket { UserId = userId, IsActive = true };
				await _repository.AddAsync(basket);
				await _unitOfWork.CompleteAsync();
			}

			existingBasket = await _repository.GetAsync(b => b.UserId == userId && b.IsActive);
			var mapped = _mapper.Map<BasketResponseDTO>(existingBasket);
			return CustomResponseDto<BasketResponseDTO>.Success(200, mapped);
		}

		public async Task<CustomResponseDto<BasketResponseDTO>> UpdateBasketAsync(int id, BasketRequestDTO dto)
		{
			var existingBasket = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingBasket);
			_mapper.Map(dto, existingBasket);
			var updatedEntity = await _repository.UpdateAsync(existingBasket);
			var updatedBasketDto = _mapper.Map<BasketResponseDTO>(updatedEntity);
			return CustomResponseDto<BasketResponseDTO>.Success(200, updatedBasketDto);
		}

		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			await _repository.SoftDeleteAsync(id);
			await _unitOfWork.CompleteAsync(); 
			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			await _repository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction(); 
			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<BasketResponseDTO>> AddBasketAsync(Basket basket)
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();
			var existingBasket = await _repository.GetAsync(b => b.UserId == userId && b.IsActive);

			if (existingBasket != null)
				return CustomResponseDto<BasketResponseDTO>.Fail(400, Messages.UserAlreadyHasActiveBasket);

			basket.UserId = userId;
			basket.IsActive = true;
			await _repository.AddAsync(basket);
			await _unitOfWork.CompleteAsync();

			var basketDto = _mapper.Map<BasketResponseDTO>(basket);
			return CustomResponseDto<BasketResponseDTO>.Success(201, basketDto);
		}


		private async Task<CustomResponseDto<decimal?>> HandleCouponCodeAsync(string couponCode, Basket basket, string userId)
		{
			var couponResult = await _basketBusinessRules.ValidateCouponByCodeAsync(couponCode);
			if (!couponResult.IsSuccessful)
				return CustomResponseDto<decimal?>.Fail(400, Messages.InvalidOrExpiredCoupon);
				

			var coupon = couponResult.Data;
			var couponUsageCheckResult = await _basketBusinessRules.CheckCouponUsageByCodeAsync(couponCode, userId);
			if (!couponUsageCheckResult.IsSuccessful)
				return CustomResponseDto<decimal?>.Fail(400, Messages.CouponAlreadyUsed);

			var couponUsageDto = new CouponUsageRequestDTO { CouponId = coupon.Id, UserId = userId, BasketId = basket.Id };
			await _couponUsageService.CreateCouponUsageAsync(couponUsageDto);

			basket.Coupon = coupon;
			basket.CouponId = coupon.Id;
			decimal discountAmount = await CalculateDiscount(coupon.Id, basket.TotalPrice);

			return CustomResponseDto<decimal?>.Success(200, discountAmount);
		}

		private async Task<decimal> CalculateDiscount(int couponId, decimal totalAmount)
		{
			var couponResponse = await _couponService.FetchCouponByCriteriaAsync(x => x.Id == couponId);
			if (couponResponse != null && couponResponse != null && couponResponse.ExpiryDate > DateTime.Now)
				return Math.Min(couponResponse.Amount, totalAmount);
			return 0;
		}

		private decimal CalculatePoints(decimal price, decimal pointsPercentage, decimal maxPoint)
		{
			var potentialPoints = price * (pointsPercentage / 100);
			return Math.Min(potentialPoints, maxPoint);
		}
	}
}
