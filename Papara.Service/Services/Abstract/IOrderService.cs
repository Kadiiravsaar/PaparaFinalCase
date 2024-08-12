using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface IOrderService 
	{
		Task<CustomResponseDto<OrderWithDetailResponseDTO?>> GetOrderWithDetailAsync(Expression<Func<Order, bool>> predicate,
			Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<OrderWithDetailResponseDTO>>> GetAllOrdersWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<OrderResponseDTO>> CreateOrder();

		Task<CustomResponseDto<List<OrderWithDetailResponseDTO>>> GetMyOrders();

		Task<CustomResponseDto<OrderResponseDTO>> GetAsync(
			Expression<Func<Order, bool>> predicate, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<OrderResponseDTO>>> GetListAsync(
			Expression<Func<Order, bool>>? predicate = null, Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<Order, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<OrderResponseDTO>> AddAsync(OrderRequestDTO orderRequestDTO);

		Task<CustomResponseDto<OrderResponseDTO>> UpdateAsync(int id, OrderRequestDTO orderRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}
	

}
