using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class StockRepository : GenericRepository<Stock>, IStockRepository
	{
		public StockRepository(MsSqlDbContext context) : base(context)
		{
		}
	}


	
}
