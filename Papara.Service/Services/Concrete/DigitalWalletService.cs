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
using Papara.Service.Exceptions;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class DigitalWalletService : IDigitalWalletService
	{
		private readonly IGenericRepository<DigitalWallet> _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUnitOfWork _unitOfWork;

		public DigitalWalletService(IGenericRepository<DigitalWallet> repository, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) 
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<CustomResponseDto<DigitalWalletResponseDTO>> CreateDigitalWalletAsync()
		{
			var userId = _httpContextAccessor.HttpContext.GetUserId();
			if (string.IsNullOrEmpty(userId))
				throw new AuthorizationException(Messages.UserNotAuthenticated);

			var existingWallet = await _repository.GetAsync(w => w.UserId == userId);
			if (existingWallet != null)
				return CustomResponseDto<DigitalWalletResponseDTO>.Fail(400, Messages.UserAlreadyHasDigitalWallet);

			var digitalWallet = new DigitalWallet { UserId = userId, Balance = 0, Points = 0 };

			await _repository.AddAsync(digitalWallet);
			await _unitOfWork.CompleteAsync();

			var responseDto = _mapper.Map<DigitalWalletResponseDTO>(digitalWallet);
			return CustomResponseDto<DigitalWalletResponseDTO>.Success(201, responseDto);
		}



		public async Task<CustomResponseDto<List<DigitalWalletWithDetailResponseDTO>>> GetAllDigitalWalletsWithDetailAsync(bool withDeleted = false)
		{
			var wallets = await _repository.GetListAsync(
			include: src => src.Include(d => d.User),
			withDeleted: withDeleted);

			var dtos = _mapper.Map<List<DigitalWalletWithDetailResponseDTO>>(wallets);
			return CustomResponseDto<List<DigitalWalletWithDetailResponseDTO>>.Success(200, dtos);
		}


		public async Task<CustomResponseDto<DigitalWalletResponseDTO>> GetWalletByUserId()
		{
			// Kullanıcı kimliğini al
			var userId = _httpContextAccessor.HttpContext.GetUserId();
			if (string.IsNullOrEmpty(userId))
				throw new AuthorizationException(Messages.UserNotAuthenticated);

			var wallet = await _repository.GetAsync(w => w.UserId == userId);
			if (wallet == null)
				throw new NotFoundException(Messages.DigitalWalletNotFound);

			var responseDto = _mapper.Map<DigitalWalletResponseDTO>(wallet);
			return CustomResponseDto<DigitalWalletResponseDTO>.Success(200, responseDto);
		}


		public async Task<CustomResponseDto<DigitalWalletWithDetailResponseDTO?>> GetDigitalWalletWithDetailAsync
			(Expression<Func<DigitalWallet, bool>> predicate, Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false)
		{
			var wallet = await _repository.GetAsync(
		   predicate,
		   include: src => src.Include(d => d.User),
		   withDeleted);

			if (wallet == null)
				throw new NotFoundException(Messages.DigitalWalletNotFound);

			var dto = _mapper.Map<DigitalWalletWithDetailResponseDTO>(wallet);
			return CustomResponseDto<DigitalWalletWithDetailResponseDTO?>.Success(200, dto);
		}


		public async Task<DigitalWallet> GetAsync(Expression<Func<DigitalWallet, bool>> predicate)
		{
			return await _repository.GetAsync(predicate);
		}


		public async Task AddAsync(DigitalWallet wallet)
		{
			await _repository.AddAsync(wallet);
			await _unitOfWork.CompleteAsync();
		}

		public async Task<CustomResponseDto<bool>> ReduceWalletBalance(string userId, decimal orderAmount, decimal userPointsToApply)
		{
			var wallet = await _repository.GetAsync(w => w.UserId == userId);
			if (wallet == null)
				return CustomResponseDto<bool>.Fail(404, Messages.DigitalWalletNotFound);

			// Önce puan kullanımı
			decimal amountLeft = orderAmount; // geriye kalan
			if (wallet.Points >= userPointsToApply)
			{
				wallet.Points -= userPointsToApply;
				orderAmount -= userPointsToApply;  // Puanlar TL olarak düşülüyor
			}
			else
			{
				amountLeft -= wallet.Points;
				wallet.Points = 0;
			}

			// Kalan tutar için bakiyeden düşürme
			if (amountLeft > 0)
			{
				if (wallet.Balance >= amountLeft)
				{
					wallet.Balance -= amountLeft;
				}
				else
				{
					return CustomResponseDto<bool>.Fail(400, Messages.InsufficientBalance);
				}
			}

			await _repository.UpdateAsync(wallet);
			await _unitOfWork.CompleteAsync();
			return CustomResponseDto<bool>.Success(200);
		}

		public async Task<CustomResponseDto<DigitalWalletResponseDTO>> GetAsync(
			Expression<Func<DigitalWallet, bool>> predicate, Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false)
		{
			var digitalWallet = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(digitalWallet);

			var digitalWalletDto = _mapper.Map<DigitalWalletResponseDTO>(digitalWallet);
			return CustomResponseDto<DigitalWalletResponseDTO>.Success(200, digitalWalletDto);
		}

		public async Task<CustomResponseDto<List<DigitalWalletResponseDTO>>> GetListAsync(
			Expression<Func<DigitalWallet, bool>>? predicate = null, Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false)
		{
			List<DigitalWallet> digitalWallets  = await _repository.GetListAsync(withDeleted: false);
			var digitalWalletsDto = _mapper.Map<List<DigitalWalletResponseDTO>>(digitalWallets);
			return CustomResponseDto<List<DigitalWalletResponseDTO>>.Success(200, digitalWalletsDto);
		}

		public async Task<bool> AnyAsync(Expression<Func<DigitalWallet, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}


		public async Task<CustomResponseDto<DigitalWalletResponseDTO>> AddAsync(DigitalWalletRequestDTO digitalWalletRequestDTO)
		{
			var digitalWallet = _mapper.Map<DigitalWallet>(digitalWalletRequestDTO);
			var addedDigitalWallet = await _repository.AddAsync(digitalWallet);
			await _unitOfWork.CompleteAsync();
			var digitalWalletResponseDTO = _mapper.Map<DigitalWalletResponseDTO>(addedDigitalWallet);
			return CustomResponseDto<DigitalWalletResponseDTO>.Success(201, digitalWalletResponseDTO);
		}

		public async Task<CustomResponseDto<DigitalWalletResponseDTO>> UpdateAsync(int id, DigitalWalletRequestDTO digitalWalletRequestDTO)
		{
			var existingDigitalWallet = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingDigitalWallet);
			_mapper.Map(digitalWalletRequestDTO, existingDigitalWallet); 

			var updatedDigitalWallet = await _repository.UpdateAsync(existingDigitalWallet);
			await _unitOfWork.CompleteAsync();

			var updatedDigitalWalletDto = _mapper.Map<DigitalWalletResponseDTO>(updatedDigitalWallet);
			return CustomResponseDto<DigitalWalletResponseDTO>.Success(200, updatedDigitalWalletDto);
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
