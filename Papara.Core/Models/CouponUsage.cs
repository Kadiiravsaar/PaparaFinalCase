namespace Papara.Core.Models
{
	public class CouponUsage : BaseEntity
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		public int BasketId { get; set; } // Kuponun hangi sepetlerde kullanıldığını izlemek için 
		public virtual Coupon Coupon { get; set; }
		public virtual AppUser User { get; set; }
		public virtual Basket Basket { get; set; }

	}
}
	