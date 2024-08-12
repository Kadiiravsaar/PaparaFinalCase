using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
		{
			var result = await _authService.RegisterAsync(registerRequest);
			return Ok(result);

		}


		[HttpPost("adminRegister")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> AdminRegister([FromBody] RegisterRequestDTO registerRequest)
		{
			var result = await _authService.AdminRegisterAsync(registerRequest);
			return Ok(result);

		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
		{
			var result = await _authService.LoginAsync(loginRequest);
			return Ok(result);   

		}
		
	}
}
