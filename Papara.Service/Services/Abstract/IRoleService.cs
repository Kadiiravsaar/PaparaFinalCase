using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;

namespace Papara.Service.Services.Abstract
{
	public interface IRoleService
	{
		Task<CustomResponseDto<RoleResponseDto>> CreateRoleAsync(RoleRequestDto roleRequestDto );
		Task<CustomResponseDto<RoleResponseDto>> UpdateRoleAsync(string roleName, RoleRequestDto roleRequestDto);
		Task<CustomResponseDto<RoleResponseDto>> DeleteRoleAsync(string roleName);
		Task<CustomResponseDto<List<RoleResponseDto>>> GetAllRolesAsync();
		
	}
}
