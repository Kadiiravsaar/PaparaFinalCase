using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface ICategoryService 
	{
		Task<IEnumerable<int>> GetCategoryIdsAsync();

		Task<CustomResponseDto<CategoryWithDetailResponseDTO?>> GetCategoryWithDetailAsync(Expression<Func<Category, bool>> predicate,
			Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<CategoryResponseDTO>> GetAsync(
			Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<CategoryWithDetailResponseDTO>>> GetAllCategoriesWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<List<CategoryResponseDTO>>> GetListAsync(
			Expression<Func<Category, bool>>? predicate = null, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<Category, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<CategoryResponseDTO>> AddAsync(CategoryRequestDTO categoryRequestDTO);

		Task<CustomResponseDto<CategoryResponseDTO>> UpdateAsync(int id,CategoryRequestDTO categoryRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}

}
