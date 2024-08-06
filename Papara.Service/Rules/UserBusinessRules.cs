using Microsoft.AspNetCore.Identity;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Rules
{
	public class UserBusinessRules
	{
		private readonly UserManager<AppUser> _userManager;

		public UserBusinessRules(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task EnsureUserExistsAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				throw new Exception(Messages.UserNotFound);
		}

		public void EnsureNotNull(object obj, string message)
		{
			if (obj == null)
				throw new Exception(message);
		}
	}

}
