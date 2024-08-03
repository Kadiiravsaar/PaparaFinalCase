using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		public OrderRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
