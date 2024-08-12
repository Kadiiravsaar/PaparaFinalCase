using Microsoft.EntityFrameworkCore;
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
		public async Task<IEnumerable<int>> GetCategoryIdsAsync()
		{
			// Tüm aktif kategorilerin ID'lerini döndüren bir LINQ sorgusu yazdım
			return await _context.Categories.Select(c => c.Id).ToListAsync();
		}

		public async Task<bool> CategoryHasProducts(int categoryId)
		{
			return await _context.ProductCategories.AnyAsync(pc => pc.CategoryId == categoryId && pc.IsActive);
		}
	}
}
