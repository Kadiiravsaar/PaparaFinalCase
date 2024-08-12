using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class RolesController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> CreateRole(RoleRequestDto roleRequestDto)
		{
			var result = await _roleService.CreateRoleAsync(roleRequestDto);
			return Ok(result);
		}

		[HttpPut("Update")]
		public async Task<IActionResult> UpdateRole(string roleName, RoleRequestDto roleRequestDto)
		{
			var result = await _roleService.UpdateRoleAsync(roleName, roleRequestDto);
			return Ok(result);
		}

		[HttpDelete("Delete")]
		public async Task<IActionResult> DeleteRole(string roleName)
		{
			var result = await _roleService.DeleteRoleAsync(roleName);

			return Ok(result);

		}

		[HttpGet("List")]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _roleService.GetAllRolesAsync();
			return Ok(roles);
		}
	}
}
