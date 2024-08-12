using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Papara.Core.Models;
using Papara.Service.Exceptions;

namespace Papara.Service.Rules
{
	public class RoleBusinessRules
	{
		private readonly RoleManager<AppRole> _roleManager;
		
		public RoleBusinessRules(RoleManager<AppRole> roleManager)
		{
			_roleManager = roleManager;
			
		}


		/// <summary>
		///  İş Kuralı: Rolün benzersiz olup olmadığını kontrol et
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		/// <exception cref="BusinessException"></exception>
		public async Task RoleNameShouldBeUnique(string roleName)
		{
			var roleExists = await _roleManager.RoleExistsAsync(roleName);
			if (roleExists)
				throw new BusinessException("RoleAlreadyExists");

		}

		/// <summary>
		///  İş Kuralı: Rolün var olup olmadığını kontrol et
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		/// <exception cref="BusinessException"></exception>
		public async Task RoleShouldExistWhenUpdatedOrDeleted(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
				throw new BusinessException("RoleNotFound");
		}
	}
}
