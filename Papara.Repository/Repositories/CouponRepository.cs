using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Repository.Context;
using System.Linq.Expressions;

namespace Papara.Repository.Repositories
{
	public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
	{
		private readonly MsSqlDbContext _context;
		public CouponRepository(MsSqlDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Coupon> GetCouponAsync(Expression<Func<Coupon, bool>> predicate)
		{
			return await _context.Coupons
						 .Where(predicate)
						 .AsNoTracking()  // Takibi engelle
						 .FirstOrDefaultAsync();
		}

	}
}
