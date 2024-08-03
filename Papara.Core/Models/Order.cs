namespace Papara.Core.Models
{
	
	public class Order : BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal? CouponAmount { get; set; } // Kuponla indirilen tutar.
		public decimal? PointsUsed { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
			

		public int? CouponId { get; set; } // İlişkili kuponun Id'si, nullable olabilir.
		public virtual Coupon Coupon { get; set; } // Kupon detayı.
	}

	
}


