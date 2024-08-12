using Papara.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.Repositories
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		Task<IEnumerable<int>> GetCategoryIdsAsync();
		Task<bool> CategoryHasProducts(int categoryId);
	}
}
