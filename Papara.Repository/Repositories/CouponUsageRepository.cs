using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class CouponUsageRepository : GenericRepository<CouponUsage>, ICouponUsageRepository
	{
		public CouponUsageRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
