using Microsoft.EntityFrameworkCore;
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

		public async Task<List<Product>> GetProductsByNameAsync(string name)
		{
			name = name.ToLower();
			return await _context.Products
				.Where(p => p.Name.ToLower().Contains(name))
				.ToListAsync();
		}

	}
}
