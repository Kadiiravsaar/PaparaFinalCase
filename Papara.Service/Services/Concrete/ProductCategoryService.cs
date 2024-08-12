using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using System.Linq.Expressions;

namespace Papara.Service.Services.Concrete
{
	public class ProductCategoryService : IProductCategoryService
	{
		private readonly IGenericRepository<ProductCategory> _repository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public ProductCategoryService(IGenericRepository<ProductCategory> repository, IMapper mapper, IUnitOfWork unitOfWork) 
		{
			_repository = repository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CustomResponseDto<ProductCategoryResponseDTO>> AddAsync(ProductCategoryRequestDTO productCategoryRequestDTO)
		{
			var productCategory = _mapper.Map<ProductCategory>(productCategoryRequestDTO);

			var addedProductCategory = await _repository.AddAsync(productCategory);
			await _unitOfWork.CompleteAsync();

			var productCategoryResponseDTO = _mapper.Map<ProductCategoryResponseDTO>(addedProductCategory);
			return CustomResponseDto<ProductCategoryResponseDTO>.Success(201, productCategoryResponseDTO);
		}

		public async Task<bool> AnyAsync(Expression<Func<ProductCategory, bool>>? predicate = null, bool withDeleted = false)
		{
			return await _repository.AnyAsync(predicate);
		}

		public async Task<CustomResponseDto<List<ProductCategoryWithDetailResponseDTO>>> GetAllProductCategoriesWithDetailAsync(bool withDeleted = false)
		{
			var productCategories = await _repository.GetListAsync(
				include: pc => pc.Include(p => p.Product).Include(c => c.Category),
				withDeleted: withDeleted);

			var dtos = _mapper.Map<List<ProductCategoryWithDetailResponseDTO>>(productCategories);
			return CustomResponseDto<List<ProductCategoryWithDetailResponseDTO>>.Success(200, dtos);
		}

		public async Task<CustomResponseDto<ProductCategoryResponseDTO>> GetAsync(
			Expression<Func<ProductCategory, bool>> predicate, Func<IQueryable<ProductCategory>, IIncludableQueryable<ProductCategory, object>>? 
			include = null, bool withDeleted = false)
		{
			var productCategory = await _repository.GetAsync(predicate, include: pc => pc.Include(p => p.Product).Include(c => c.Category),
				withDeleted: withDeleted); ;

			BusinessRules.CheckEntityExists(productCategory);

			var productCategoryDto = _mapper.Map<ProductCategoryResponseDTO>(productCategory);
			return CustomResponseDto<ProductCategoryResponseDTO>.Success(200, productCategoryDto);
		}

		public async Task<CustomResponseDto<List<ProductCategoryResponseDTO>>> GetListAsync(
			Expression<Func<ProductCategory, bool>>? predicate = null, Func<IQueryable<ProductCategory>, 
				IIncludableQueryable<ProductCategory, object>>? include = null, bool withDeleted = false)
		{

			List<ProductCategory> productCategories  = await _repository.GetListAsync(withDeleted: false, include: pc => pc.Include(p => p.Product).Include(c => c.Category));
			var productCategoriesDto = _mapper.Map<List<ProductCategoryResponseDTO>>(productCategories);
			return CustomResponseDto<List<ProductCategoryResponseDTO>>.Success(200, productCategoriesDto);
		}

		public async Task<CustomResponseDto<ProductCategoryWithDetailResponseDTO?>> GetProductCategoryWithDetailAsync(
			Expression<Func<ProductCategory, bool>> predicate, Func<IQueryable<ProductCategory>, IIncludableQueryable<ProductCategory, object>>? include = null, 
			bool withDeleted = false)
		{
			var productCategory = await _repository.GetAsync(
				predicate,
				include: pc => pc.Include(p => p.Product).Include(c => c.Category),
				withDeleted: withDeleted);

			if (productCategory == null)
				return CustomResponseDto<ProductCategoryWithDetailResponseDTO?>.Fail(404,Messages.ProductCategoryNotFound);

			var dto = _mapper.Map<ProductCategoryWithDetailResponseDTO>(productCategory);
			return CustomResponseDto<ProductCategoryWithDetailResponseDTO?>.Success(200, dto);
		}



		public async Task<CustomResponseDto<bool>> HardDeleteAsync(int id)
		{
			await _repository.HardDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();

			return CustomResponseDto<bool>.Success(204, true);
		}


		public async Task<CustomResponseDto<bool>> SoftDeleteAsync(int id)
		{
			await _repository.SoftDeleteAsync(id);
			await _unitOfWork.CompleteWithTransaction();

			return CustomResponseDto<bool>.Success(204, true);
		}

		public async Task<CustomResponseDto<ProductCategoryResponseDTO>> UpdateProductCategoryAsync(int id, ProductCategoryRequestDTO productCategoryRequestDTO)
		{
			var existingProductCategory = await _repository.GetAsync(x => x.Id == id);

			BusinessRules.CheckEntityExists(existingProductCategory);
			_mapper.Map(productCategoryRequestDTO, existingProductCategory); 

			var updatedProductCategory = await _repository.UpdateAsync(existingProductCategory);
			await _unitOfWork.CompleteWithTransaction();

			var updatedProductCategoryDto = _mapper.Map<ProductCategoryResponseDTO>(updatedProductCategory);
			return CustomResponseDto<ProductCategoryResponseDTO>.Success(200, updatedProductCategoryDto);
		}
	}

}

