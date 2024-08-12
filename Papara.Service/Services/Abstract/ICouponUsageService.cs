using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;


namespace Papara.Service.Services.Abstract
{
	public interface ICouponUsageService 
	{
		Task<CustomResponseDto<CouponUsageWithDetailResponseDTO>> GetCouponUsageWithDetailAsync(
			Expression<Func<CouponUsage, bool>> predicate, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<CouponUsageResponseDTO>> GetAsync(
			Expression<Func<CouponUsage, bool>> predicate, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<CouponUsageWithDetailResponseDTO>>> GetAllCouponUsagesWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<List<CouponUsageResponseDTO>>> GetListAsync(
			Expression<Func<CouponUsage, bool>>? predicate = null, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<CouponUsage, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<CouponUsageWithDetailResponseDTO>> CreateCouponUsageAsync(CouponUsageRequestDTO requestDto);

		Task<CustomResponseDto<CouponUsageResponseDTO>> AddAsync(CouponUsageRequestDTO couponUsageRequestDTO);

		Task<CustomResponseDto<CouponUsageResponseDTO>> UpdateAsync(int id, CouponUsageRequestDTO couponUsageRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}
}
