using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;
using System.Text.Json;

namespace Papara.Service.Services.Concrete
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IDistributedCache _distributedCache;

		public CategoryService(ICategoryRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IDistributedCache distributedCache)
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_distributedCache = distributedCache;
		}

		public async Task<CustomResponseDto<CategoryWithDetailResponseDTO?>> GetCategoryWithDetailAsync(Expression<Func<Category, bool>> predicate,
			Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false)
		{
			var category = await _repository.GetAsync(
				predicate,
				include: x => x.Include(c => c.ProductCategories.Where(i => i.IsActive))
					  .ThenInclude(pc => pc.Product), 
				withDeleted);

			if (category == null)
				return CustomResponseDto<CategoryWithDetailResponseDTO?>.Fail(404, "Id not found");
			

			var dto = _mapper.Map<CategoryWithDetailResponseDTO>(category);
			return CustomResponseDto<CategoryWithDetailResponseDTO?>.Success(200, dto);
		}

		public async Task<CustomResponseDto<List<CategoryWithDetailResponseDTO>>> GetAllCategoriesWithDetailAsync(bool withDeleted = false)
		{
			var categories = await _repository.GetListAsync(
				include: x => x.Include(c => c.ProductCategories.Where(i => i.IsActive))
							   .ThenInclude(pc => pc.Product), // Product bilgilerini de include ediyoruz
				withDeleted: withDeleted
			);

			var dtos = _mapper.Map<List<CategoryWithDetailResponseDTO>>(categories);
			return CustomResponseDto<List<CategoryWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<CategoryResponseDTO>> GetAsync(
			Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false)
		{
			var category = await _repository.GetAsync(predicate);

			BusinessRules.CheckEntityExists(category);

			var categoryDto = _mapper.Map<CategoryResponseDTO>(category);
			return CustomResponseDto<CategoryResponseDTO>.Success(200, categoryDto);
		}

		public async Task<CustomResponseDto<List<CategoryResponseDTO>>> GetListAsync(
			Expression<Func<Category, bool>>? predicate = null, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false)
		{
			var cacheKey = "categories";
			string serializedCategories = await _distributedCache.GetStringAsync(cacheKey);

			if (!string.IsNullOrEmpty(serializedCategories))
			{
				var categories = JsonSerializer.Deserialize<List<CategoryResponseDTO>>(serializedCategories);
				return CustomResponseDto<List<CategoryResponseDTO>>.Success(200, categories);
			}

			var categoryList = await _repository.GetListAsync(predicate, include, withDeleted);
			var mappedCategories = _mapper.Map<List<CategoryResponseDTO>>(categoryList);
			await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(mappedCategories), new DistributedCacheEntryOptions
			{
				AbsoluteExpiration = DateTime.Now.AddDays(1),
				SlidingExpiration = TimeSpan.FromHours(1)
			});

			return CustomResponseDto<List<CategoryResponseDTO>>.Success(200, mappedCategories);
		}

		public async Task<bool> AnyAsync(Expression<Func<Category, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}

		public async Task<CustomResponseDto<CategoryResponseDTO>> UpdateAsync(int id, CategoryRequestDTO categoryRequestDTO)
		{
			var existingCategory = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingCategory);
			_mapper.Map(categoryRequestDTO, existingCategory);

			var updatedCategory = await _repository.UpdateAsync(existingCategory);
			await _unitOfWork.CompleteWithTransaction();

			await _distributedCache.RemoveAsync("categories"); // Cache'i temizle


			var updatedCategoryDto = _mapper.Map<CategoryResponseDTO>(updatedCategory);
			return CustomResponseDto<CategoryResponseDTO>.Success(200, updatedCategoryDto);
		}


		public async Task<CustomResponseDto<CategoryResponseDTO>> AddAsync(CategoryRequestDTO categoryRequestDTO)
		{
			var category = _mapper.Map<Category>(categoryRequestDTO);
			var addedCategory = await _repository.AddAsync(category);
			await _unitOfWork.CompleteAsync(); 

			await _distributedCache.RemoveAsync("categories"); 
			var categoryResponseDTO = _mapper.Map<CategoryResponseDTO>(addedCategory);
			return CustomResponseDto<CategoryResponseDTO>.Success(201, categoryResponseDTO);
		}



		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			var categoryHasProducts = await _repository.CategoryHasProducts(id);
			if (categoryHasProducts)
			{
				return CustomResponseDto<bool>.Fail(400, "Cannot delete category because it has products.");
			}
			await _repository.SoftDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();
			await _distributedCache.RemoveAsync("categories"); 
			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			var categoryHasProducts = await _repository.CategoryHasProducts(id);
			if (categoryHasProducts)
			{
				return CustomResponseDto<bool>.Fail(400, "Cannot delete category because it has products.");
			}
			await _repository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();
			await _distributedCache.RemoveAsync("categories");
			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<IEnumerable<int>> GetCategoryIdsAsync()
		{
			return await _repository.GetCategoryIdsAsync();
		}
	}
}
