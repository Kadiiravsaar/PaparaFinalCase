using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;

namespace Papara.Service.Services.Abstract
{
	public interface IAuthService
	{
		Task<CustomResponseDto<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO registerRequest);
		Task<CustomResponseDto<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest);
		Task<CustomResponseDto<RegisterResponseDTO>> AdminRegisterAsync(RegisterRequestDTO registerRequest);
	}
}
