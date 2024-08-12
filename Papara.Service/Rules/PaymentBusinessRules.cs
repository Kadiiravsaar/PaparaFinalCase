using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using Papara.Repository.Context;
using Papara.Service.Constants;
using System.Security.Claims;

namespace Papara.Service.Rules
{
	public class PaymentBusinessRules
	{

		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PaymentBusinessRules(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}


		public async Task<AppUser> ValidateUserAndWalletAsync(string userId, UserManager<AppUser> userManager, MsSqlDbContext context)
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
				throw new Exception(Messages.UserNotFound);

			user = await context.Users.Include(u => u.DigitalWallet).FirstOrDefaultAsync(u => u.Id == userId);
			if (user.DigitalWallet == null)
				throw new Exception(Messages.UserDoesNotHaveDigitalWallet);

			return user;
		}

		public async Task<string> GetUserIdFromTokenAsync()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
				throw new Exception(Messages.UserNotAuthenticated);

			return userId;
		}

		public bool ValidateCardDetails(string cardNumber, string cvv, DateTime expiryDate)
		{
			if (cardNumber.Length != 16 || !long.TryParse(cardNumber, out _))
				return false;
			if (cvv.Length != 3 || !int.TryParse(cvv, out _))
				return false;
			if (expiryDate <= DateTime.Today)
				return false;

			return true;
		}
	}
}
