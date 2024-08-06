using Papara.Core.DTOs;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using Papara.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public async Task<CustomResponseDto<bool>> CheckCouponUsageAsync(int couponId, string userId)
		{
			var couponUsageCheck = await _couponUsageService.GetCouponUsageWithDetailAsync(
				cu => cu.CouponId == couponId && cu.UserId == userId);

			if (couponUsageCheck.Data != null)
			{
				throw new BusinessException(Messages.CouponAlreadyHasBeen);
			}

			return CustomResponseDto<bool>.Success(200);
		}

		public async Task<CustomResponseDto<Coupon>> ValidateCouponAsync(int couponId)
		{
			var coupon = await _couponService.GetCouponAsync(
				c => c.Id == couponId && c.ExpiryDate > DateTime.Now);

			if (coupon == null)
			{
				return CustomResponseDto<Coupon>.Fail(404, "Geçersiz veya süresi dolmuş kupon.");
			}

			return CustomResponseDto<Coupon>.Success(200, coupon);
		}
	}
}
