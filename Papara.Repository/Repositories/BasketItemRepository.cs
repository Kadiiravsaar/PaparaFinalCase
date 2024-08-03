using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
	{
		public BasketItemRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
