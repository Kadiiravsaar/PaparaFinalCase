using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{

	public interface IProductCategoryService
	{
		Task<CustomResponseDto<ProductCategoryWithDetailResponseDTO?>> GetProductCategoryWithDetailAsync(Expression<Func<ProductCategory, bool>> predicate,
			Func<IQueryable<ProductCategory>, IIncludableQueryable<ProductCategory, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<ProductCategoryWithDetailResponseDTO>>> GetAllProductCategoriesWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<ProductCategoryResponseDTO>> GetAsync(
			Expression<Func<ProductCategory, bool>> predicate, Func<IQueryable<ProductCategory>, IIncludableQueryable<ProductCategory, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<ProductCategoryResponseDTO>>> GetListAsync(
			Expression<Func<ProductCategory, bool>>? predicate = null, Func<IQueryable<ProductCategory>, IIncludableQueryable<ProductCategory, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<ProductCategory, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<ProductCategoryResponseDTO>> AddAsync(ProductCategoryRequestDTO productCategoryRequestDTO);

		Task<CustomResponseDto<ProductCategoryResponseDTO>> UpdateProductCategoryAsync(int id, ProductCategoryRequestDTO productCategoryRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}
}
