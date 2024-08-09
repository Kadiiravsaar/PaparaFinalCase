using Microsoft.EntityFrameworkCore.Query;
using Papara.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.Repositories
{

	public interface IGenericRepository<TEntity> where TEntity : BaseEntity
	{
		Task<TEntity?> GetAsync(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
			bool withDeleted = false
		);

		Task<List<TEntity>> GetListAsync(
			Expression<Func<TEntity, bool>>? predicate = null,
			Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
			bool withDeleted = false
		);


		Task<bool> AnyAsync(
			Expression<Func<TEntity, bool>>? predicate = null,
			bool withDeleted = false
		);

		Task<TEntity> AddAsync(TEntity entity);

		Task<TEntity> UpdateAsync(TEntity entity);
	
		Task SoftDeleteAsync(int id);

		Task HardDeleteAsync(int id);

	}
}

