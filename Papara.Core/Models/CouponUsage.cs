namespace Papara.Core.Models
{
	public class CouponUsage : BaseEntity
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		public DateTime UsedDate { get; set; }

		public virtual Coupon Coupon { get; set; }
		public virtual AppUser User { get; set; }

	}
}
	