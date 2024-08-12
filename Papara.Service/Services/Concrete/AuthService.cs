using Microsoft.AspNetCore.Identity;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Data;
using System.IdentityModel.Tokens.Jwt;


namespace Papara.Service.Services.Concrete
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly AuthBusinessRules _authBusinessRules;
		private readonly IBasketService _basketService;

		public AuthService(UserManager<AppUser> userManager, ITokenService tokenService, AuthBusinessRules authBusinessRules, IBasketService basketService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_authBusinessRules = authBusinessRules;
			_basketService = basketService;
		}

		public async Task<CustomResponseDto<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO registerRequest)
		{
			const string userRoleName = "User";

			var user = new AppUser
			{
				UserName = registerRequest.UserName,
				Email = registerRequest.Email,
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName
			};

			var createUserResult = await _userManager.CreateAsync(user, registerRequest.Password); // Kullanıcıyı kaydetme işlemim
			if (!createUserResult.Succeeded)
				return CustomResponseDto<RegisterResponseDTO>.Fail(400, createUserResult.Errors.Select(e => e.Description).ToList());

			await _authBusinessRules.EnsureRoleExists(userRoleName); // Rol kontrolü ve oluşturma işlemini iş kuralında yaptım

			await _userManager.AddToRoleAsync(user, userRoleName); // Kullanıcıya rol atama işlemi yaptım

			var roles = await _userManager.GetRolesAsync(user); 
			var token = await _tokenService.CreateToken(user);

			await _userManager.UpdateAsync(user);
			var registerResponse = new RegisterResponseDTO
			{
				Message= Messages.RegistrationSuccess
			};
			return CustomResponseDto<RegisterResponseDTO>.Success(200, registerResponse);
		}

		public async Task<CustomResponseDto<RegisterResponseDTO>> AdminRegisterAsync(RegisterRequestDTO registerRequest)
		{
			const string userRoleName = "Admin";

			var user = new AppUser
			{
				UserName = registerRequest.UserName,
				Email = registerRequest.Email,
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName
			};

			var createUserResult = await _userManager.CreateAsync(user, registerRequest.Password); // Kullanıcıyı kaydetme işlemim
			if (!createUserResult.Succeeded)
				return CustomResponseDto<RegisterResponseDTO>.Fail(400, createUserResult.Errors.Select(e => e.Description).ToList());

			await _authBusinessRules.EnsureRoleExists(userRoleName); // Rol kontrolü ve oluşturma işlemini iş kuralında yaptım

			await _userManager.AddToRoleAsync(user, userRoleName); // Kullanıcıya rol atama işlemi yaptım

			var roles = await _userManager.GetRolesAsync(user);
			var token = await _tokenService.CreateToken(user);

			await _userManager.UpdateAsync(user);

			var registerResponse = new RegisterResponseDTO
			{
				Message = Messages.AdminRegistrationSuccess

			};
			return CustomResponseDto<RegisterResponseDTO>.Success(200, registerResponse);
		}


		public async Task<CustomResponseDto<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
		{
			AppUser user = await _userManager.FindByEmailAsync(loginRequest.Email);

			if (user == null)
				return CustomResponseDto<LoginResponseDTO>.Fail(404, Messages.UserNotFound);

			await _authBusinessRules.CheckPasswordAsync(user, loginRequest.Password);

			if (user.Baskets == null)
				await _basketService.EmptyBasket(user.Id);

			IList<string> roles = await _userManager.GetRolesAsync(user);

			JwtSecurityToken createToken = await _tokenService.CreateToken(user);

			await _userManager.UpdateAsync(user);
			await _userManager.UpdateSecurityStampAsync(user);

			string _token = new JwtSecurityTokenHandler().WriteToken(createToken);
			await _userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", _token);

			LoginResponseDTO loginResponse = new LoginResponseDTO { Token = _token,  Expiration = createToken.ValidTo };

			return CustomResponseDto<LoginResponseDTO>.Success(200, loginResponse);
		}
	}
}
