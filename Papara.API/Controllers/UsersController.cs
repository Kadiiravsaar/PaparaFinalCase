using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetList()
		{
			var userList = await _userService.UserList();
			return Ok(userList);
		}

		[HttpGet("Id")]
		public async Task<IActionResult> GetUser(string userId)
		{	
			var user = await _userService.GetUserByIdAsync(userId);
			return Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> AddUser(RegisterRequestDTO registerRequestDTO )
		{
			var user = await _userService.AddUserByAdminAsync(registerRequestDTO);
			return Ok(user);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteUser(string userId)
		{
			var user = await _userService.HardDeleteUserAsync(userId);
			return Ok(user);
		}

		[HttpPatch]
		public async Task<IActionResult> SoftDeleteUser(string userId)
		{
			var user = await _userService.SoftDeleteUserAsync(userId);
			return Ok(user);
		}


		[HttpPut]
		public async Task<IActionResult> UpdateUser(string userId, UpdateUserRequestDTO updateUserRequestDTO )
		{
			var user = await _userService.UpdateUserAsync(userId, updateUserRequestDTO);
			return Ok(user);
		}

	}
}
