using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Extensions;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;


namespace Papara.Service.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IBasketService _basketService;
		private readonly IMapper _mapper;
		private readonly UserBusinessRules _userBusinessRules;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserService(UserManager<AppUser> userManager, IMapper mapper, UserBusinessRules userBusinessRules, IHttpContextAccessor httpContextAccessor, IBasketService basketService)
		{
			_userManager = userManager;
			_mapper = mapper;
			_userBusinessRules = userBusinessRules;
			_httpContextAccessor = httpContextAccessor;
			_basketService = basketService;
		}
		public async Task<CustomResponseDto<AppUserResponseDTO>> GetUserByIdAsync(string userId)
		{

			_userBusinessRules.EnsureNotNull(userId, Messages.FillAllFields);

			await _userBusinessRules.EnsureUserExistsAsync(userId);
			var user = await _userManager.FindByIdAsync(userId);

			var userDetailDto = _mapper.Map<AppUserResponseDTO>(user);

			return CustomResponseDto<AppUserResponseDTO>.Success(200, userDetailDto);

		}
		public async Task<CustomResponseDto<List<AppUserResponseDTO>>> UserList()
		{
			var users = await _userManager.Users.Where(x => x.IsActive).ToListAsync();
			var usersDto = _mapper.Map<List<AppUserResponseDTO>>(users);

			return CustomResponseDto<List<AppUserResponseDTO>>.Success(200, usersDto);

		}



		public async Task<CustomResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserRequestDTO updateUserRequestDTO)
		{
			var user = await _userManager.FindByIdAsync(userId);
			
			await _userBusinessRules.EnsureUserExistsAsync(userId);  
			await _userBusinessRules.CheckEmailConflictAsync(updateUserRequestDTO.Email, userId);  


			user.UserName = updateUserRequestDTO.UserName;
			user.FirstName = updateUserRequestDTO.FirstName;
			user.LastName = updateUserRequestDTO.LastName;
			user.Email = updateUserRequestDTO.Email;

			// Kullanıcının şifresini güncelle
			if (!string.IsNullOrWhiteSpace(updateUserRequestDTO.Password))
			{
				// Mevcut şifreyi sıfırlayıp yeni şifre belirle
				var removePasswordResult = await _userManager.RemovePasswordAsync(user);
				if (!removePasswordResult.Succeeded)
				return CustomResponseDto<bool>.Fail(400, removePasswordResult.Errors.Select(e => e.Description).ToList());

				var addPasswordResult = await _userManager.AddPasswordAsync(user, updateUserRequestDTO.Password);
				if (!addPasswordResult.Succeeded)
				return CustomResponseDto<bool>.Fail(400, addPasswordResult.Errors.Select(e => e.Description).ToList());
			}

			// Kullanıcı bilgilerini güncelle
			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
				return CustomResponseDto<bool>.Fail(400, result.Errors.Select(e => e.Description).ToList());

			return CustomResponseDto<bool>.Success(200, true);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteUserAsync(string userId)
		{
			var user = await _userManager.Users
								 .Include(u => u.DigitalWallet)
								 .Include(u => u.Orders.Where(b => b.IsActive))  
								 .Include(u => u.Baskets.Where(b=>b.IsActive)) 
								 .FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
				return CustomResponseDto<bool>.Fail(404, Messages.UserNotFound);

			if (user.Orders.Any() || user.Baskets.Any() || (user.DigitalWallet != null))
				return CustomResponseDto<bool>.Fail(400, "User has related data and cannot be deleted.");

			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded)
				return CustomResponseDto<bool>.Fail(400, result.Errors.Select(e => e.Description).ToList());
			return CustomResponseDto<bool>.Success(200, true);
		}


		public async Task<CustomResponseDto<bool>> SoftDeleteUserAsync(string userId)
		{
			var user = await _userManager.Users
								.Include(u => u.Orders)
								.Include(u => u.Baskets)
								.Include(u => u.DigitalWallet)
								.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
				return CustomResponseDto<bool>.Fail(404, Messages.UserNotFound);
			

			user.IsActive = false;
			if (user.Orders != null)
			foreach (var order in user.Orders)
				{
					order.IsActive = false;
					order.DeletedDate = DateTime.UtcNow.AddHours(3);
				}


			if (user.Baskets != null)
				foreach (var basket in user.Baskets)
				{
					basket.IsActive = false;
					basket.DeletedDate = DateTime.UtcNow.AddHours(3);

				}

			if (user.DigitalWallet != null)
				user.DigitalWallet.IsActive = false;


			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
				return CustomResponseDto<bool>.Fail(400, result.Errors.Select(e => e.Description).ToList());
			
			return CustomResponseDto<bool>.Success(200, true);
		}

		public async Task<CustomResponseDto<AppUserResponseDTO>> AddUserByAdminAsync(RegisterRequestDTO registerDto)
		{

			await _userBusinessRules.CheckEmailAlreadyInUseAsync(registerDto.Email);

			var user = new AppUser
			{
				UserName = registerDto.UserName,
				Email = registerDto.Email,
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				CreatedDate = DateTime.UtcNow,
				IsActive = true
			};

			var result = await _userManager.CreateAsync(user, registerDto.Password);
			if (!result.Succeeded)
			return CustomResponseDto<AppUserResponseDTO>.Fail(400, result.Errors.Select(e => e.Description).ToList());


			var userDto = _mapper.Map<AppUserResponseDTO>(user);
			return CustomResponseDto<AppUserResponseDTO>.Success(201, userDto);
		}

	}
}
