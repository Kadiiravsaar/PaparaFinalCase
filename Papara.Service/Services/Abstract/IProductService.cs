using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;


namespace Papara.Service.Services.Abstract
{
	public interface IProductService 
	{
		Task<CustomResponseDto<ProductWithDetailResponseDTO?>> GetProductWithDetailAsync(Expression<Func<Product, bool>> predicate,
			Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false);
		Task<CustomResponseDto<List<ProductWithDetailResponseDTO>>> GetAllProductsWithDetailAsync(bool withDeleted = false);
		Task<CustomResponseDto<bool>> RemoveCategoryFromProduct(int productId, int categoryId);

		Task<CustomResponseDto<ProductResponseDTO>> UpdateStockAsync(int id, ProductRequestDTO productRequestDTO);
		Task<CustomResponseDto<ProductResponseDTO>> GetAsync(
			Expression<Func<Product, bool>> predicate, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<ProductResponseDTO>>> GetProductsByNameAsync(string name);
		Task<CustomResponseDto<List<ProductResponseDTO>>> GetListAsync(
			Expression<Func<Product, bool>>? predicate = null, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<Product, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<ProductResponseDTO>> AddProductAsync(ProductRequestDTO productRequestDTO);

		Task<CustomResponseDto<ProductResponseDTO>> UpdateProductAsync(int id, ProductRequestDTO productRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);


	}
}
