using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Repository.Repositories;
using Papara.Repository.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Exceptions;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Services.Concrete
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _repository;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public ProductService(IProductRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICategoryService categoryService)
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_categoryService = categoryService;
		}


		public async Task<CustomResponseDto<ProductWithDetailResponseDTO?>> GetProductWithDetailAsync(
			Expression<Func<Product, bool>> predicate, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false)
		{
			var product = await _repository.GetAsync(
			predicate,
			include: x => x.Include(c => c.ProductCategories)
				  .ThenInclude(pc => pc.Category), 

			withDeleted);

			if (product == null)
				return CustomResponseDto<ProductWithDetailResponseDTO?>.Fail(404, Messages.ProductNotFound);

			var dto = _mapper.Map<ProductWithDetailResponseDTO>(product);
			return CustomResponseDto<ProductWithDetailResponseDTO?>.Success(200, dto);
		}


		public async Task<CustomResponseDto<List<ProductWithDetailResponseDTO>>> GetAllProductsWithDetailAsync(bool withDeleted = false)
		{
			var products = await _repository.GetListAsync(
					include: x => x.Include(c => c.ProductCategories)
								   .ThenInclude(pc => pc.Category), 
					withDeleted: withDeleted
				);

			var dtos = _mapper.Map<List<ProductWithDetailResponseDTO>>(products);
			return CustomResponseDto<List<ProductWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<ProductResponseDTO>> GetAsync(Expression<Func<Product, bool>> predicate, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false)
		{
			var product = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(product);

			var productDto = _mapper.Map<ProductResponseDTO>(product);
			return CustomResponseDto<ProductResponseDTO>.Success(200, productDto);
		}

		public async Task<CustomResponseDto<List<ProductResponseDTO>>> GetListAsync(Expression<Func<Product, bool>>? predicate = null, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, bool withDeleted = false)
		{
			List<Product> products = await _repository.GetListAsync(withDeleted: false);
			var productsDto = _mapper.Map<List<ProductResponseDTO>>(products);
			return CustomResponseDto<List<ProductResponseDTO>>.Success(200, productsDto);
		}

		public async Task<bool> AnyAsync(Expression<Func<Product, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}

		
		public async Task<CustomResponseDto<List<ProductResponseDTO>>> GetProductsByNameAsync(string name)
		{
			var products = await _repository.GetProductsByNameAsync(name);
			var productsDto = _mapper.Map<List<ProductResponseDTO>>(products);

			return CustomResponseDto<List<ProductResponseDTO>>.Success(200, productsDto);
		}



		private async Task ValidateCategoryIds(IEnumerable<int> categoryIds)
		{
			var validCategoryIds = await _categoryService.GetCategoryIdsAsync();

			// Verilen kategori ID'leri ile geçerli kategori ID'lerini karşılaştır.
			var invalidIds = categoryIds.Except(validCategoryIds).ToList();
			if (invalidIds.Any())
				throw new ArgumentException($"Invalid category IDs: {string.Join(", ", invalidIds)}. These categories do not exist.");
		}

		public async Task<CustomResponseDto<ProductResponseDTO>> AddProductAsync(ProductRequestDTO productRequestDTO)
		{
			// Ürün eklerken kategori listesinin boş olmamasını kontrol et
			if (productRequestDTO.CategoryIds == null || !productRequestDTO.CategoryIds.Any())
				throw new ArgumentException(Messages.AtLeastOneCategoryIdRequired);

			await ValidateCategoryIds(productRequestDTO.CategoryIds);

			var product = _mapper.Map<Product>(productRequestDTO);


			product.ProductCategories = productRequestDTO.CategoryIds
				.Select(cid => new ProductCategory { CategoryId = cid, ProductId = product.Id }).ToList();

			var addedProduct = await _repository.AddAsync(product);
			await _unitOfWork.CompleteAsync(); 

			var productResponseDTO = _mapper.Map<ProductResponseDTO>(addedProduct);
			return CustomResponseDto<ProductResponseDTO>.Success(201, productResponseDTO);
		}


		public async Task<CustomResponseDto<bool>> RemoveCategoryFromProduct(int productId, int categoryId)
		{
			var product = await _repository.GetAsync(p => p.Id == productId, p => p.Include(x => x.ProductCategories));
			if (product == null)
				return CustomResponseDto<bool>.Fail(404, Messages.ProductNotFound);

			var productCategory = product.ProductCategories.FirstOrDefault(pc => pc.CategoryId == categoryId);
			if (productCategory == null)
				return CustomResponseDto<bool>.Fail(404, Messages.CategoryNotLinkedToProduct);

			product.ProductCategories.Remove(productCategory);

			await _repository.UpdateAsync(product);
			await _unitOfWork.CompleteAsync();

			return CustomResponseDto<bool>.Success(200, true);
		}


		public async Task<CustomResponseDto<ProductResponseDTO>> UpdateStockAsync(int id, ProductRequestDTO productRequestDTO)
		{
			var existingProduct = await _repository.GetAsync(x => x.Id == id);

			if (existingProduct == null)
				return CustomResponseDto<ProductResponseDTO>.Fail(404, Messages.ProductNotFound);

			_mapper.Map(productRequestDTO, existingProduct); 

			await _repository.UpdateAsync(existingProduct);
			await _unitOfWork.CompleteAsync(); 

			var updatedDto = _mapper.Map<ProductResponseDTO>(existingProduct);
			return CustomResponseDto<ProductResponseDTO>.Success(200, updatedDto);
		}



		public async Task<CustomResponseDto<ProductResponseDTO>> UpdateProductAsync(int id, ProductRequestDTO productRequestDTO)
		{

			var existingProduct = await _repository.GetAsync(
				x => x.Id == id && x.IsActive,
				include: pc => pc.Include(p => p.ProductCategories.Where(c => c.IsActive))
					  .ThenInclude(p => p.Product));
			BusinessRules.CheckEntityExists(existingProduct);

			if (productRequestDTO.CategoryIds != null && productRequestDTO.CategoryIds.Any())
				await ValidateCategoryIds(productRequestDTO.CategoryIds);

			_mapper.Map(productRequestDTO, existingProduct); 


			var currentCategoryIds = existingProduct.ProductCategories
										.Where(pc => pc.IsActive) 
										.Select(pc => pc.CategoryId)
										.ToList();

			if (!currentCategoryIds.Any())
				throw new Exception(Messages.ProductNotLinkedToAnyCategory);
			
			var categoriesToRemove = existingProduct.ProductCategories.Where(pc => !productRequestDTO.CategoryIds.Contains(pc.CategoryId)).ToList();
			foreach (var category in categoriesToRemove)
			{
				existingProduct.ProductCategories.Remove(category);
			}

			var categoryIdsToAdd = productRequestDTO.CategoryIds.Where(cId => !currentCategoryIds.Contains(cId)).ToList();

			foreach (var categoryId in categoryIdsToAdd)
			{
				existingProduct.ProductCategories.Add(new ProductCategory { ProductId = id, CategoryId = categoryId });
			}

			var updatedProduct = await _repository.UpdateAsync(existingProduct);
			await _unitOfWork.CompleteWithTransaction();

			var updatedProductDto = _mapper.Map<ProductResponseDTO>(updatedProduct);
			return CustomResponseDto<ProductResponseDTO>.Success(200, updatedProductDto);
		}


		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			var product = await _repository.GetAsync(p => p.Id == id, include: p => p.Include(x => x.ProductCategories));
			if (product == null) 
				return CustomResponseDto<bool>.Fail(404, Messages.ProductNotFound);

			product.IsActive = false; 
			foreach (var pc in product.ProductCategories)
			{
				pc.IsActive = false; 
			}

			await _repository.UpdateAsync(product); 
			await _unitOfWork.CompleteWithTransaction(); 
			return CustomResponseDto<bool>.Success(204, true);
		}



		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			var product = await _repository.GetAsync(p => p.Id == id);
			if (product == null) return CustomResponseDto<bool>.Fail(404, Messages.ProductNotFound);

			await _repository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();
			return CustomResponseDto<bool>.Success(204, true);
		}
	}

}

	