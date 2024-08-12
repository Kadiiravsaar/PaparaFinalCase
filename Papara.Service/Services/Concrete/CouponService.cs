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
using Papara.Service.Exceptions;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class CouponService : ICouponService
	{
		private readonly ICouponRepository _couponRepository; // Repository türünü güncelle
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public CouponService(ICouponRepository couponRepository, IMapper mapper, IUnitOfWork unitOfWork)

		{
			_couponRepository = couponRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CustomResponseDto<List<CouponWithDetailResponseDTO>>> GetAllCouponsWithDetailAsync(bool withDeleted = false)
		{
			var coupons = await _couponRepository.GetListAsync(
			include: x => x.Include(c => c.Baskets)
						   .ThenInclude(o => o.Items).ThenInclude(x=>x.Product)
						   .Include(c => c.Usages),
			withDeleted: withDeleted
		);

			var dtos = _mapper.Map<List<CouponWithDetailResponseDTO>>(coupons);
			return CustomResponseDto<List<CouponWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<CouponWithDetailResponseDTO?>> GetCouponWithDetailAsync(Expression<Func<Coupon, bool>> predicate, Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false)
		{
			var coupon = await _couponRepository.GetAsync(
		   predicate,
		   include: x => x.Include(c => c.Baskets)
						  .ThenInclude(o => o.Items)
						  .Include(c => c.Usages),
		   withDeleted);

			if (coupon == null)
				return CustomResponseDto<CouponWithDetailResponseDTO?>.Fail(404, Messages.CouponNotFound);


			var dto = _mapper.Map<CouponWithDetailResponseDTO>(coupon);
			return CustomResponseDto<CouponWithDetailResponseDTO?>.Success(200, dto);
		}

		public async Task DeactivateExpiredCoupons()
		{
			var expiredCoupons = await _couponRepository.GetListAsync(c => c.ExpiryDate <= DateTime.Now && c.IsActive);
			foreach (var coupon in expiredCoupons)
			{
				coupon.IsActive = false;
			}
			await _unitOfWork.CompleteAsync();
		}

		public async Task<Coupon> FetchCouponByCriteriaAsync(Expression<Func<Coupon, bool>> predicate)
		{
			return await _couponRepository.GetCouponAsync(predicate);
		}


		public async Task<CustomResponseDto<CouponResponseDTO>> GetAsync(
			Expression<Func<Coupon, bool>> predicate, Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false)
		{
			var coupon = await _couponRepository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(coupon);

			var couponDto = _mapper.Map<CouponResponseDTO>(coupon);
			return CustomResponseDto<CouponResponseDTO>.Success(200, couponDto);
		}

		public async Task<CustomResponseDto<CouponResponseDTO>> CreateCouponAsync(CouponRequestDTO couponRequest)
		{
			var existingCoupon = await _couponRepository.GetCouponByCodeAsync(couponRequest.CouponCode);
			if (existingCoupon != null)
				return CustomResponseDto<CouponResponseDTO>.Fail(400, Messages.CouponCodeAlreadyExists);
			

			var coupon = _mapper.Map<Coupon>(couponRequest);
			await _couponRepository.AddAsync(coupon);
			await _unitOfWork.CompleteAsync();

			var couponResponse = _mapper.Map<CouponResponseDTO>(coupon);
			return CustomResponseDto<CouponResponseDTO>.Success(201, couponResponse);
		}


		public async Task<CustomResponseDto<CouponResponseDTO>> AddAsync(CouponRequestDTO couponRequestDTO)
		{
			var coupon = _mapper.Map<Coupon>(couponRequestDTO);
			var addedCoupon = await _couponRepository.AddAsync(coupon);
			await _unitOfWork.CompleteAsync(); // Değişiklikleri commit et
			var addedCouponDto = _mapper.Map<CouponResponseDTO>(addedCoupon);
			return CustomResponseDto<CouponResponseDTO>.Success(201, addedCouponDto);
		}

		public async Task<CustomResponseDto<List<CouponResponseDTO>>> GetListAsync(
			Expression<Func<Coupon, bool>>? predicate = null, Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null, bool withDeleted = false)
		{
			List<Coupon> coupons = await _couponRepository.GetListAsync(withDeleted: false);
			var couponsDto = _mapper.Map<List<CouponResponseDTO>>(coupons);
			return CustomResponseDto<List<CouponResponseDTO>>.Success(200, couponsDto);
		}

		public async Task<bool> AnyAsync(Expression<Func<Coupon, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _couponRepository.AnyAsync(predicate);

		}

		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			var isCouponUsed = await _couponRepository.AnyAsync(c=>c.Id == id);
			if (isCouponUsed)
				throw new ClientSideException(Messages.CouponCannotBeDeleted);

			await _couponRepository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();

			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			await _couponRepository.SoftDeleteAsync(id);
			await _unitOfWork.CompleteAsync(); 

			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<CouponResponseDTO>> UpdateAsync(int id, CouponRequestDTO couponRequestDTO)
		{
			var existingCoupon = await _couponRepository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingCoupon);

			_mapper.Map(couponRequestDTO, existingCoupon);
			var updatedCoupon = await _couponRepository.UpdateAsync(existingCoupon);

			await _unitOfWork.CompleteWithTransaction();

			var updatedCouponDto = _mapper.Map<CouponResponseDTO>(updatedCoupon);
			return CustomResponseDto<CouponResponseDTO>.Success(200, updatedCouponDto);
		}
	}
}


