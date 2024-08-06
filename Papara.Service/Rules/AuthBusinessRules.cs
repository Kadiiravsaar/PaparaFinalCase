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
	public class AuthBusinessRules
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AuthBusinessRules(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}


		public async Task CheckPasswordAsync(AppUser user, string password)	
		{
			var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
			if (!result.Succeeded)	
				throw new BusinessException(Messages.InvalidUsernameOrPassword);
			
		}

		public async Task EnsureRoleExists(string roleName)
		{
			var roleExists = await _roleManager.RoleExistsAsync(roleName);
			if (roleExists)
			{
				return; 
			}

			var role = new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() };
			var result = await _roleManager.CreateAsync(role);

			if (!result.Succeeded)
			{
				var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
				throw new InvalidOperationException(Messages.UserRoleNotExists + ": " + errors);
			}
		}
	}
}
