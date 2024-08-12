using Papara.Core.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Papara.Service.Services.Abstract
{
	public interface ITokenService
	{
		Task<JwtSecurityToken> CreateToken(AppUser user);
	}
}
