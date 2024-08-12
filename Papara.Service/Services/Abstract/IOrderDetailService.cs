using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{

	public interface IOrderDetailService
	{
		Task<CustomResponseDto<OrderDetailWithDetailResponseDTO?>> GetOrderDetailWithDetailAsync(Expression<Func<OrderDetail, bool>> predicate,
			Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<OrderDetailResponseDTO>> GetAsync(
			Expression<Func<OrderDetail, bool>> predicate, Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<OrderDetailWithDetailResponseDTO>>> GetAllOrderDetailsWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<List<OrderDetailResponseDTO>>> GetListAsync
			(Expression<Func<OrderDetail, bool>>? predicate = null, Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<OrderDetail, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<OrderDetailResponseDTO>> UpdateAsync(int id, OrderDetailRequestDTO orderDetailRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}

}
