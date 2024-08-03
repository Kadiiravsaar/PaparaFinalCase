using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class DigitalWalletRepository : GenericRepository<DigitalWallet>, IDigitalWalletRepository
	{
		public DigitalWalletRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
