using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Papara.Core.Models;
using Papara.Service.Services.Abstract;
using Papara.Service.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Papara.Service.Services.Concrete
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly TokenSettings tokenSettings;

		public TokenService(IOptions<TokenSettings> options, UserManager<AppUser> userManager)
		{
			_userManager = userManager;
			tokenSettings = options.Value;
		}

		public async Task<JwtSecurityToken> CreateToken(AppUser user)
		{
			var roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),  // Kullanıcı ID'sini "NameIdentifier" olarak ekledim
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.SecurityKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: tokenSettings.Issuer,
				audience: tokenSettings.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(tokenSettings.TokenValidityInMunitues),
				signingCredentials: creds
			);
			return token;
		}
	}
}
