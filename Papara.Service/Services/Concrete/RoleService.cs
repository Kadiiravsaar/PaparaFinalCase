using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papara.Core.DTOs.Response;
using Papara.Core.DTOs;
using Papara.Core.Models;
using Papara.Service.Services.Abstract;
using Papara.Core.DTOs.Request;
using Papara.Service.Rules;


namespace Papara.Service.Services.Concrete
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IMapper _mapper;

		private readonly RoleBusinessRules _roleBusinessRules; 
		public RoleService(RoleManager<AppRole> roleManager, RoleBusinessRules roleBusinessRules, IMapper mapper)
		{
			_roleManager = roleManager;
			_mapper = mapper;
			_roleBusinessRules = roleBusinessRules;
		}

		public async Task<CustomResponseDto<RoleResponseDto>> CreateRoleAsync(RoleRequestDto roleRequestDto)
		{
			var role = _mapper.Map<AppRole>(roleRequestDto);
			await _roleManager.CreateAsync(role);
			var roleDto = _mapper.Map<RoleResponseDto>(role);
			return CustomResponseDto<RoleResponseDto>.Success(201, roleDto);

		}

		public async Task<CustomResponseDto<RoleResponseDto>> UpdateRoleAsync(string roleName, RoleRequestDto roleRequestDto)
		{
			var role = await _roleManager.FindByNameAsync(roleName);

			_mapper.Map(roleRequestDto, role);
			role.NormalizedName = roleRequestDto.Name.ToUpper();

			await _roleManager.UpdateAsync(role);


			var roleDto = _mapper.Map<RoleResponseDto>(role);
			return CustomResponseDto<RoleResponseDto>.Success(200, roleDto);

		}

		public async Task<CustomResponseDto<RoleResponseDto>> DeleteRoleAsync(string roleName)
		{

			await _roleBusinessRules.RoleShouldExistWhenUpdatedOrDeleted(roleName); 
			var role = await _roleManager.FindByNameAsync(roleName);
			await _roleManager.DeleteAsync(role);

			return CustomResponseDto<RoleResponseDto>.Success(200);

		}

		public async Task<CustomResponseDto<List<RoleResponseDto>>> GetAllRolesAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var roleDtos = _mapper.Map<List<RoleResponseDto>>(roles);

			return CustomResponseDto<List<RoleResponseDto>>.Success(200, roleDtos);
		}
		
	}
}
