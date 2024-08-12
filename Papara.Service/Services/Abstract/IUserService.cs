using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;

namespace Papara.Service.Services.Abstract
{
	public interface IUserService
	{
		Task<CustomResponseDto<List<AppUserResponseDTO>>> UserList();
		Task<CustomResponseDto<AppUserResponseDTO>> GetUserByIdAsync(string userId);

		/// <summary>
		/// Login olan kullanıcı bilgilerini dön
		/// </summary>
		/// <returns></returns>

		Task<CustomResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserRequestDTO updateDto);
		Task<CustomResponseDto<bool>> HardDeleteUserAsync(string userId);

		Task<CustomResponseDto<bool>> SoftDeleteUserAsync(string userId);
		Task<CustomResponseDto<AppUserResponseDTO>> AddUserByAdminAsync(RegisterRequestDTO registerDto);

	}
}
