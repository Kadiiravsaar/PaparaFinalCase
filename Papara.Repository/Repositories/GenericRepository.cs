using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Papara.Core.Repositories;
using Papara.Core.Models;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
	{
		private readonly MsSqlDbContext _context;

		public GenericRepository(MsSqlDbContext context)
		{
			_context = context;
		}

		public async Task<TEntity> AddAsync(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Provided entity must not be null.");
			}
			entity.CreatedDate = DateTime.UtcNow;

			await _context.Set<TEntity>().AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}


		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			entity.UpdatedDate = DateTime.UtcNow;
			_context.Update(entity);
			await _context.SaveChangesAsync();
			return entity;
		}
		public async Task SoftDeleteAsync(int id)
		{
			TEntity? entityId = await GetAsync(x => x.Id == id);
			if (entityId != null)
			{
				entityId.IsActive = false;
				_context.Update(entityId);
			}
		}

		public async Task HardDeleteAsync(int id)
		{
			TEntity entityId = await GetAsync(x => x.Id == id);
			if (entityId != null)
				_context.Remove(entityId);
		}

		public async Task<IList<TEntity>> DeleteRangeAsync(IList<TEntity> entities, bool forceDelete = false)
		{
			if (forceDelete == false)
			{
				foreach (TEntity entity in entities)
				{
					entity.IsActive = false;
					entity.DeletedDate = DateTime.UtcNow;
					_context.Update(entity);
				}
			}
			else
				_context.RemoveRange(entities);

			await _context.SaveChangesAsync();
			return null;
		}

		public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false)
		{
			IQueryable<TEntity> queryable = _context.Set<TEntity>();
			if (!withDeleted)
				queryable = queryable.Where(e => e.IsActive);
			if (include != null)
				queryable = include(queryable);
			return await queryable.FirstOrDefaultAsync(predicate);
		}

		public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false)
		{
			IQueryable<TEntity> queryable = _context.Set<TEntity>();
			if (!withDeleted)
				queryable = queryable.Where(e => e.IsActive);

			if (include != null)
				queryable = include(queryable);
			if (predicate != null)
				queryable = queryable.Where(predicate);
			return await queryable.ToListAsync();
		}

		public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false)
		{

			IQueryable<TEntity> queryable = _context.Set<TEntity>();
			if (withDeleted)
				queryable = queryable.Where(e => e.IsActive);
			if (predicate != null)
				queryable = queryable.Where(predicate);
			return await queryable.AnyAsync();
		}

		public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false)
		{
			IQueryable<TEntity> queryable = _context.Set<TEntity>();
			if (!withDeleted)
				queryable = queryable.Where(e => e.IsActive);

			if (predicate != null)
				queryable = queryable.Where(predicate);
			return await queryable.ToListAsync();
		}
	}
}
