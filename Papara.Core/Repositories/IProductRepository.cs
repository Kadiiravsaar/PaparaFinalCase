using Papara.Core.DTOs;
using Papara.Core.Models;

namespace Papara.Core.Repositories
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<List<Product>> GetProductsByNameAsync(string name);
	}
}
