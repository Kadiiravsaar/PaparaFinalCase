using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
