using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface IBasketItemService 
	{
		Task<CustomResponseDto<BasketItemWithDetailResponseDTO?>> GetBasketItemWithDetailAsync(Expression<Func<BasketItem, bool>> predicate,
			Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<BasketItemResponseDTO>> GetAsync(
		Expression<Func<BasketItem, bool>> predicate, Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<BasketItemResponseDTO>>> GetListAsync(
			Expression<Func<BasketItem, bool>>? predicate = null, Func<IQueryable<BasketItem>, IIncludableQueryable<BasketItem, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<BasketItemWithDetailResponseDTO>>> GetAllBasketItemsWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<BasketItemResponseDTO>> AddItemToBasketAsync(BasketItemRequestDTO basketItemRequest);

		Task<CustomResponseDto<BasketItemResponseDTO>> UpdateBasketItemAsync(int id, BasketItemRequestDTO basketItemRequest);

		Task<CustomResponseDto<bool>> SoftDeleteBasketItemAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteBasketItemAsync(int id);
	}
}
