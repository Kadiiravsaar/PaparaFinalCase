using System.Runtime.ConstrainedExecution;

namespace Papara.Core.Models
{
	public class Coupon : BaseEntity
	{
		public string CouponCode { get; set; }  // Benzersiz kupon kodu, max 10 karakter.
		public decimal Amount { get; set; } // Kuponun değeri
		public DateTime ExpiryDate { get; set; } // Kuponun son kullanma tarihi
		public virtual ICollection<CouponUsage> Usages { get; set; } // Kuponun hangi kullanıcılar tarafından kullanıldığını takip eder.
		public virtual ICollection<Basket> Baskets { get; set; } // Kuponun hangi kullanıcılar tarafından kullanıldığını takip eder.
	}
}
