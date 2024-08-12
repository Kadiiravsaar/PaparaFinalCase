using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class OrderDetailService : IOrderDetailService
	{
		private readonly IGenericRepository<OrderDetail> _repository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public OrderDetailService(IGenericRepository<OrderDetail> repository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> AnyAsync(Expression<Func<OrderDetail, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);

		}

		public async Task<CustomResponseDto<List<OrderDetailWithDetailResponseDTO>>> GetAllOrderDetailsWithDetailAsync(bool withDeleted = false)
		{
			var orderDetails = await _repository.GetListAsync(
				include: query => query.Include(od => od.Product), 
				withDeleted: withDeleted
			);

			var dtos = _mapper.Map<List<OrderDetailWithDetailResponseDTO>>(orderDetails);
			return CustomResponseDto<List<OrderDetailWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<OrderDetailResponseDTO>> GetAsync(Expression<Func<OrderDetail, bool>> predicate, 
			Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false)
		{
			var orderDetail = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(orderDetail);

			var orderDetailDto = _mapper.Map<OrderDetailResponseDTO>(orderDetail);
			return CustomResponseDto<OrderDetailResponseDTO>.Success(200, orderDetailDto);
		}

		public async Task<CustomResponseDto<List<OrderDetailResponseDTO>>> GetListAsync(Expression<Func<OrderDetail, bool>>? predicate = null, Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false)
		{
			List<OrderDetail> orderDetails  = await _repository.GetListAsync(withDeleted: false);
			var orderDetailssDto = _mapper.Map<List<OrderDetailResponseDTO>>(orderDetails);
			return CustomResponseDto<List<OrderDetailResponseDTO>>.Success(200, orderDetailssDto);
		}

		public async Task<CustomResponseDto<OrderDetailWithDetailResponseDTO?>> GetOrderDetailWithDetailAsync(Expression<Func<OrderDetail, bool>> predicate, Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null, bool withDeleted = false)
		{
			var orderDetail = await _repository.GetAsync(
				predicate,
				include: query => query.Include(od => od.Product),
				withDeleted
			);

			if (orderDetail == null)
				return CustomResponseDto<OrderDetailWithDetailResponseDTO?>.Fail(404, Messages.OrderDetailNotFound);
			

			var dto = _mapper.Map<OrderDetailWithDetailResponseDTO>(orderDetail);
			return CustomResponseDto<OrderDetailWithDetailResponseDTO?>.Success(200, dto);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			await _repository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction(); 

			return CustomResponseDto<bool>.Success(204, true);
		}


		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			await _repository.SoftDeleteAsync(id);
			await _unitOfWork.CompleteAsync(); 

			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<OrderDetailResponseDTO>> UpdateAsync(int id, OrderDetailRequestDTO orderDetailRequestDTO)
		{
			var existingOrderDetail = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingOrderDetail);
			_mapper.Map(orderDetailRequestDTO, existingOrderDetail); 

			var updatedOrderDetail = await _repository.UpdateAsync(existingOrderDetail);
			await _unitOfWork.CompleteAsync(); 

			var updatedOrderDetailDto = _mapper.Map<OrderDetailResponseDTO>(updatedOrderDetail);
			return CustomResponseDto<OrderDetailResponseDTO>.Success(200, updatedOrderDetailDto);
		}
	}
}

