using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;

namespace Papara.Repository.Repositories
{
	public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
	{
		public CouponRepository(MsSqlDbContext context) : base(context)
		{
		}
	}
}
