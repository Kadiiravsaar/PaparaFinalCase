using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class BasketRepository : GenericRepository<Basket>, IBasketRepository
	{
		public BasketRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
