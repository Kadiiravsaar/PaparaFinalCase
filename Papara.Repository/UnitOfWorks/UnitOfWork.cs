using Papara.Core.UnitOfWorks;
using Papara.Repository.Context;


namespace Papara.Repository.UnitOfWorks
{

	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly MsSqlDbContext _context;

		public UnitOfWork(MsSqlDbContext context)
		{
			_context = context;
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		public async Task CompleteAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task CompleteWithTransaction()
		{
			using (var dbTransaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.SaveChangesAsync();
					await dbTransaction.CommitAsync();
				}
				catch (Exception ex)
				{
					await dbTransaction.RollbackAsync();
					Console.WriteLine(ex);
					throw;
				}
			}
		}
	}
}
