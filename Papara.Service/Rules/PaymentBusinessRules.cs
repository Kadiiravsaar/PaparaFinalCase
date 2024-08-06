using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
