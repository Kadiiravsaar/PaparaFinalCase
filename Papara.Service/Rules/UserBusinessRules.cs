using Microsoft.AspNetCore.Identity;
using Papara.Core.Models;
using Papara.Service.Constants;
using Papara.Service.Exceptions;

namespace Papara.Service.Rules
{
	public class UserBusinessRules
	{
		private readonly UserManager<AppUser> _userManager;

		public UserBusinessRules(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task CheckEmailAlreadyInUseAsync(string email)
		{
			var userExists = await _userManager.FindByEmailAsync(email);
			if (userExists != null)
				throw new ClientSideException(Messages.EmailAlreadyInUse); 
		}
		public async Task CheckEmailConflictAsync(string email, string userId)
		{
			var existingUser = await _userManager.FindByEmailAsync(email);
			if (existingUser != null && existingUser.Id != userId)
				throw new ClientSideException(Messages.EmailAlreadyInUse);
		}
		public async Task EnsureUserExistsAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				throw new ClientSideException(Messages.UserNotFound);
		}

		public void EnsureNotNull(object obj, string message)
		{
			if (obj == null)
				throw new Exception(message);
		}
	}

}
