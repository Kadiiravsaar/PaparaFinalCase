namespace Papara.Core.Models
{
	public class Basket : BaseEntity
	{
		public string UserId { get; set; }
		public virtual AppUser User { get; set; }

		public decimal TotalPrice { get; set; } // Sepetteki tüm ürünlerin toplam tutarı
		public decimal FinalPrice { get; set; } // İndirim sonrası fiyat
		public decimal? DiscountAmount { get; set; } // İndirim sonrası fiyat
		//public decimal PointUsed { get; set; }

		public decimal? PointsEarned { get; set; } // Bu siparişten kazanılacak puan

		public int? CouponId { get; set; }
		public virtual Coupon Coupon { get; set; } // İlişkili kupon, kullanıcı sepete kupon uygulamışsa

		public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

	}
}
