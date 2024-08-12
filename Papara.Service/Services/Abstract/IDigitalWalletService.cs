using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Service.Services.Abstract
{
	public interface IDigitalWalletService 
	{
		Task<CustomResponseDto<DigitalWalletWithDetailResponseDTO?>> GetDigitalWalletWithDetailAsync(Expression<Func<DigitalWallet, bool>> predicate,
			Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<DigitalWalletWithDetailResponseDTO>>> GetAllDigitalWalletsWithDetailAsync(bool withDeleted = false);

		Task<CustomResponseDto<DigitalWalletResponseDTO>> GetWalletByUserId();
		
		Task<CustomResponseDto<DigitalWalletResponseDTO>> CreateDigitalWalletAsync();


		/// <summary>
		/// Basket işlemlerinde doğrudan CustomResponse yerine DigitalWallet nesnesi gelmeli o yüzden yazıldı
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task<DigitalWallet> GetAsync(Expression<Func<DigitalWallet, bool>> predicate);


		/// <summary>
		/// sepette doğrudan hesap cüzdanı çekmek için yazıldı
		/// Generic repodan gelen veri mapplenmiş dönüşmüş veri.
		/// </summary>
		/// <param name="wallet"></param>
		/// <returns></returns>
		Task AddAsync(DigitalWallet wallet);

		Task<CustomResponseDto<bool>> ReduceWalletBalance(string userId, decimal orderAmount, decimal userPointsToApply);

		Task<CustomResponseDto<DigitalWalletResponseDTO>> GetAsync(Expression<Func<DigitalWallet, bool>> predicate, Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false);

		Task<CustomResponseDto<List<DigitalWalletResponseDTO>>> GetListAsync(Expression<Func<DigitalWallet, bool>>? predicate = null, Func<IQueryable<DigitalWallet>, IIncludableQueryable<DigitalWallet, object>>? include = null, bool withDeleted = false);

		Task<bool> AnyAsync(Expression<Func<DigitalWallet, bool>>? predicate = null, bool withDeleted = false);

		Task<CustomResponseDto<DigitalWalletResponseDTO>> AddAsync(DigitalWalletRequestDTO digitalWalletRequestDTO);

		Task<CustomResponseDto<DigitalWalletResponseDTO>> UpdateAsync(int id, DigitalWalletRequestDTO digitalWalletRequestDTO);

		Task<CustomResponseDto<bool>> SoftDeleteAsync(int id);

		Task<CustomResponseDto<bool>> HardDeleteAsync(int id);

	}

}
