using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface ICouponService 
	{
		Task<CustomResponseDto<CouponWithDetailResponseDTO?>> GetCouponWithDetailAsync(Expression<Func<Coupon, bool>> predicate,
			Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<CouponResponseDTO>> GetAsync(
		Expression<Func<Coupon, bool>> predicate, Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false);

		Task<Coupon> FetchCouponByCriteriaAsync(Expression<Func<Coupon, bool>> predicate);

		Task<CustomResponseDto<CouponResponseDTO>> CreateCouponAsync(CouponRequestDTO couponRequest);

		Task<CustomResponseDto<List<CouponWithDetailResponseDTO>>> GetAllCouponsWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<List<CouponResponseDTO>>> GetListAsync(
			Expression<Func<Coupon, bool>>? predicate = null, Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<CouponResponseDTO>> AddAsync(CouponRequestDTO couponRequestDTO);

		Task<bool> AnyAsync(Expression<Func<Coupon, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<CouponResponseDTO>> UpdateAsync(int id, CouponRequestDTO couponRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}
}
