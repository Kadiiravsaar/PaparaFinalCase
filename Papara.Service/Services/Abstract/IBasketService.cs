using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface IBasketService
	{
		Task<CustomResponseDto<BasketWithDetailResponseDTO?>> GetBasketWithDetailAsync(Expression<Func<Basket, bool>> predicate,
			Func<IQueryable<Basket>, IIncludableQueryable<Basket, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<BasketResponseDTO?>> GetBasketAsync(Expression<Func<Basket, bool>> predicate, Func<IQueryable<Basket>,
			IIncludableQueryable<Basket, object>>? include = null, bool withDeleted = false);
		Task<CustomResponseDto<List<BasketResponseDTO>>> GetAllBasketsAsync(bool withDeleted = false);

		Task<CustomResponseDto<List<BasketWithDetailResponseDTO>>> GetAllBasketsWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<BasketResponseDTO>> EmptyBasket(string userId);

		Task<CustomResponseDto<BasketResponseDTO>> AddBasketAsync(Basket basket);

		Task<CustomResponseDto<BasketResponseDTO>> UpdateBasketAsync(int id, BasketRequestDTO dto);

		Task<CustomResponseDto<BasketWithDetailResponseDTO>> GetCurrentUserBasket();

		Task<CustomResponseDto<BasketWithDetailResponseDTO>> CalculateBasketItemsPriceAsync(BasketRequestDTO basketRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);
	}

}
