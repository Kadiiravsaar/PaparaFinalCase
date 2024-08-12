using Papara.Core.DTOs;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using Papara.Service.Services.Abstract;

namespace Papara.Service.Rules
{
	public class BasketBusinessRules
	{
		private readonly ICouponUsageService _couponUsageService;
		private readonly ICouponService _couponService;

		public BasketBusinessRules(ICouponUsageService couponUsageService, ICouponService couponService)
		{
			_couponUsageService = couponUsageService;
			_couponService = couponService;
		}


		public async Task<CustomResponseDto<bool>> CheckCouponUsageByCodeAsync(string couponCode, string userId)
		{
			var coupon = await _couponService.FetchCouponByCriteriaAsync(c => c.CouponCode == couponCode);
			if (coupon == null)
				throw new ClientSideException(Messages.CouponNotFound);

			var couponUsageCheck = await _couponUsageService.GetCouponUsageWithDetailAsync(
				cu => cu.CouponId == coupon.Id && cu.UserId == userId);

			if (couponUsageCheck.Data == null)
				return CustomResponseDto<bool>.Success(200);

			throw new ClientSideException(Messages.CouponAlreadyHasBeen);
		}

		public async Task<CustomResponseDto<Coupon>> ValidateCouponByIdAsync(int couponId)
		{
			var coupon = await _couponService.FetchCouponByCriteriaAsync(
				c => c.Id == couponId && c.ExpiryDate > DateTime.Now);

			if (coupon == null)
				throw new ClientSideException(Messages.InvalidOrExpiredCoupon);
			return CustomResponseDto<Coupon>.Success(200, coupon);
		}

		public async Task<CustomResponseDto<Coupon>> ValidateCouponByCodeAsync(string couponCode)
		{
			var coupon = await _couponService.FetchCouponByCriteriaAsync(
				c => c.CouponCode == couponCode && c.ExpiryDate > DateTime.Now);

			if (coupon == null)
				return CustomResponseDto<Coupon>.Fail(404, Messages.InvalidOrExpiredCoupon);

			return CustomResponseDto<Coupon>.Success(200, coupon);
		}

	}
}
