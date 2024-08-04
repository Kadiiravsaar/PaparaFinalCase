namespace Papara.Core.Models
{
	public class Basket : BaseEntity
	{
		public string UserId { get; set; }
		public virtual AppUser User { get; set; }

		public decimal TotalPrice { get; set; } // Sepetteki tüm ürünlerin toplam tutarı

		public int? CouponId { get; set; }
		public virtual Coupon Coupon { get; set; } // İlişkili kupon, kullanıcı sepete kupon uygulamışsa

		public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

	}
}
