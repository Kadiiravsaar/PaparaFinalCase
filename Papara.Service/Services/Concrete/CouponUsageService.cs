using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Repository.Repositories;
using Papara.Repository.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class CouponUsageService : ICouponUsageService
	{
		private readonly IGenericRepository<CouponUsage> _repository;
		private readonly IMapper _mapper;
		private readonly ICouponService _couponService;
		private readonly IUnitOfWork _unitOfWork;
		public CouponUsageService(IGenericRepository<CouponUsage> repository, IMapper mapper, IUnitOfWork unitOfWork, ICouponService couponService) 
		{
			_repository = repository;
			_mapper = mapper;
			_couponService = couponService;
			_unitOfWork = unitOfWork;
		}

		public async Task<CustomResponseDto<List<CouponUsageWithDetailResponseDTO>>> GetAllCouponUsagesWithDetailAsync(bool withDeleted = false)
		{
			var couponUsages = await _repository.GetListAsync(
			  include: cu => cu.Include(cu => cu.Coupon).Include(cu => cu.User).Include(cu => cu.Basket),
			  withDeleted: withDeleted
		  );

			if (couponUsages == null || !couponUsages.Any())
				return CustomResponseDto<List<CouponUsageWithDetailResponseDTO>>.Fail(404, Messages.NoCouponUsagesFound);

			var dto = _mapper.Map<List<CouponUsageWithDetailResponseDTO>>(couponUsages);
			return CustomResponseDto<List<CouponUsageWithDetailResponseDTO>>.Success(200, dto);
		}

		public async Task<CustomResponseDto<CouponUsageWithDetailResponseDTO>> GetCouponUsageWithDetailAsync(Expression<Func<CouponUsage, bool>> predicate, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false)
		{
			var couponUsage = await _repository.GetAsync(
			predicate, 
			include: cu => cu.Include(cu => cu.Coupon).Include(cu => cu.User).Include(cu=>cu.Basket),
			withDeleted
		);

			if (couponUsage == null)
				return CustomResponseDto<CouponUsageWithDetailResponseDTO>.Fail(404,  Messages.CouponUsageNotFound);
			
			var dto = _mapper.Map<CouponUsageWithDetailResponseDTO>(couponUsage);
			return CustomResponseDto<CouponUsageWithDetailResponseDTO>.Success(200, dto);
		}
		public async Task<CustomResponseDto<bool>> CheckCouponUsageByCodeAsync(string couponCode, string userId)
		{
			var coupon = await _repository.GetAsync(cu => cu.Coupon.CouponCode == couponCode && cu.UserId == userId);
			if (coupon != null)
				return CustomResponseDto<bool>.Fail(409, Messages.CouponAlreadyUsedByUser);

			return CustomResponseDto<bool>.Success(200);
		}

		public async Task<CustomResponseDto<CouponUsageWithDetailResponseDTO>> CreateCouponUsageAsync(CouponUsageRequestDTO requestDto)
		{
			var existingUsage = await _repository.GetAsync(
				cu => cu.CouponId == requestDto.CouponId && cu.UserId == requestDto.UserId);

			if (existingUsage != null)
				return CustomResponseDto<CouponUsageWithDetailResponseDTO>.Fail(409, Messages.CouponAlreadyUsedByUser);

			var coupon = await _couponService.FetchCouponByCriteriaAsync(c => c.Id == requestDto.CouponId && c.ExpiryDate > DateTime.UtcNow);
			if (coupon == null)
				return CustomResponseDto<CouponUsageWithDetailResponseDTO>.Fail(404, Messages.CouponNotFoundOrExpired);

			var newUsage = _mapper.Map<CouponUsage>(requestDto);

			await _repository.AddAsync(newUsage);
			await _unitOfWork.CompleteAsync();

			var responseDto = _mapper.Map<CouponUsageWithDetailResponseDTO>(newUsage);
			return CustomResponseDto<CouponUsageWithDetailResponseDTO>.Success(200, responseDto);
		}

		public async Task<CustomResponseDto<CouponUsageResponseDTO>> GetAsync(Expression<Func<CouponUsage, bool>> predicate, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false)
		{
			var couponUsage = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(couponUsage);

			var couponUsageDto = _mapper.Map<CouponUsageResponseDTO>(couponUsage);
			return CustomResponseDto<CouponUsageResponseDTO>.Success(200, couponUsageDto);
		}

		public async Task<CustomResponseDto<List<CouponUsageResponseDTO>>> GetListAsync(Expression<Func<CouponUsage, bool>>? predicate = null, Func<IQueryable<CouponUsage>, IIncludableQueryable<CouponUsage, object>>? include = null, bool withDeleted = false)
		{
			List<CouponUsage> couponUsages = await _repository.GetListAsync(withDeleted: false);
			var couponUsagesDto = _mapper.Map<List<CouponUsageResponseDTO>>(couponUsages);
			return CustomResponseDto<List<CouponUsageResponseDTO>>.Success(200, couponUsagesDto);
		}

		public async Task<bool> AnyAsync(Expression<Func<CouponUsage, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}

		public async Task<CustomResponseDto<CouponUsageResponseDTO>> AddAsync(CouponUsageRequestDTO couponUsageRequestDTO)
		{
			
			var couponUsage = _mapper.Map<CouponUsage>(couponUsageRequestDTO);
			var addedCouponUsage = await _repository.AddAsync(couponUsage);
			await _unitOfWork.CompleteAsync();
			var couponUsageResponseDTO = _mapper.Map<CouponUsageResponseDTO>(addedCouponUsage);
			return CustomResponseDto<CouponUsageResponseDTO>.Success(201, couponUsageResponseDTO);
		}

		public async Task<CustomResponseDto<CouponUsageResponseDTO>> UpdateAsync(int id, CouponUsageRequestDTO couponUsageRequestDTO)
		{
			var existingCouponUsage = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingCouponUsage);
			_mapper.Map(couponUsageRequestDTO, existingCouponUsage); 

			var updatedCouponUsage = await _repository.UpdateAsync(existingCouponUsage);
			await _unitOfWork.CompleteAsync();

			var updatedCouponUsageDto = _mapper.Map<CouponUsageResponseDTO>(updatedCouponUsage);
			return CustomResponseDto<CouponUsageResponseDTO>.Success(200, updatedCouponUsageDto);
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
	}

}

