using System.Runtime.ConstrainedExecution;

namespace Papara.Core.Models
{
	//public class Coupon : BaseEntity
	//{
	//	public string CouponCode { get; set; }
	//	public decimal Amount { get; set; } 
	//	public DateTime ExpiryDate { get; set; } 
	//	public virtual ICollection<Order> Orders { get; set; } // Kuponun kullanılabileceği siparişler
	//}

	// Bir kupon birden fazla siparişte kullanılabileceği için (ancak her sipariş için tek kullanımlık),
	// bir kupon birden fazla siparişle ilişkilendirilemez. Bunun yerine, siparişe bir kupon atanır ve kullanıldı olarak işaretlenir.

	public class Coupon : BaseEntity
	{
		public string CouponCode { get; set; }  // Benzersiz kupon kodu, max 10 karakter.
		public decimal Amount { get; set; } // Kuponun değeri
		public DateTime ExpiryDate { get; set; } // Kuponun son kullanma tarihi

		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<CouponUsage> Usages { get; set; }
	}

	// Kupon kullanımını doğrudan Order (sipariş) modeliyle ilişkilendirmek,
	// iş modeliniz açısından en mantıklı yaklaşım olacaktır.
	// Çünkü bir kupon genelde sipariş seviyesinde uygulanır ve siparişin genel tutarına etki eder.
	// Sipariş detayları (OrderDetail), genellikle siparişe dahil olan özel ürünlerin detaylarını yönetir ve her bir ürün için ayrı ayrı kupon uygulamak yerine,
	// kuponun tüm siparişe uygulanması daha yaygındır.

	//İlişki Kurulumu
	// Order ve Coupon: Her sipariş bir kupon kodu içerebilir ve bu kupon, siparişin toplam tutarından indirim sağlar.
	// CouponUsage: Kupon kullanımını takip etmek için oluşturulacak CouponUsage tablosu,
	// hangi kullanıcının hangi kuponu kullandığını ve ne zaman kullandığını kaydeder.

	// Bu yapıyla, bir kuponun yalnızca bir siparişle ilişkilendirilmesini sağlamış oluyoruz ve bir kullanıcı aynı kuponu birden fazla kez kullanamaz.
	// Ayrıca, kuponların genel geçerliği korunurken, her kullanım kaydedilerek iş kuralları güçlendirilmiş olu

}
