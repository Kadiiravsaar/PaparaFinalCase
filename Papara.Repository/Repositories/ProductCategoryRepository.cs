using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
	{
		public ProductCategoryRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
