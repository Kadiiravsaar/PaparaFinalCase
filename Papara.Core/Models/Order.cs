namespace Papara.Core.Models
{
	//public class Order : BaseEntity
	//{
	//	public string UserId { get; set; }
	//	public AppUser User { get; set; }
	//	public string OrderNumber { get; set; }
	//	public decimal TotalAmount { get; set; }
	//	public decimal CouponAmount { get; set; }
	//	public string? CouponCode { get; set; }
	//	public decimal PointsUsed { get; set; }
	//	public virtual ICollection<OrderDetail> OrderDetails { get; set; }
	//}


	// Bir kupon birden fazla siparişte kullanılabileceği için (ancak her sipariş için tek kullanımlık),
	// bir kupon birden fazla siparişle ilişkilendirilemez. Bunun yerine, siparişe bir kupon atanır ve kullanıldı olarak işaretlenir.


	// Bu yapıyla, bir kuponun yalnızca bir kullanıcı tarafından bir kez kullanılabilmesini sağlayabiliriz.
	// Kupon kullanıldığında IsUsed alanını true olarak güncellemek gerekecektir. Bu işlem, kupon kodunun tekrar kullanılmasını engeller ve her siparişte benzersiz olmasını sağlar.
	// Sipariş işlemi sırasında, kullanılan kupon kodunu kontrol edip IsUsed durumuna göre işlem yapmak mümkün olur.
	public class Order : BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		//public string? CouponCode { get; set; } // İlişkili kupon kodu, nullable olabilir.
		public decimal CouponAmount { get; set; } // Kuponla indirilen tutar.
		public decimal PointsUsed { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }

		public int? CouponId { get; set; } // İlişkili kuponun Id'si, nullable olabilir.
		public virtual Coupon Coupon { get; set; } // Kupon detayı.
	}

	
}


