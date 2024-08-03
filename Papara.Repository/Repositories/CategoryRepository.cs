using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;


namespace Papara.Repository.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
