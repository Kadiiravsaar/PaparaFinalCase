using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using System.Linq.Expressions;

namespace Papara.Core.Repositories
{
	public interface ICouponRepository : IGenericRepository<Coupon>
	{
		Task<Coupon> GetCouponAsync(Expression<Func<Coupon, bool>> predicate);
		
	}
}
