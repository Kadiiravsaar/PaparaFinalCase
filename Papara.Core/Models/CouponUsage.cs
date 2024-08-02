namespace Papara.Core.Models
{
	public class CouponUsage : BaseEntity
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		public DateTime UsedDate { get; set; }


		// kullanıcı ile kupon arasında ilişki var zaten buradan çıkarılabilir

		// yada 

		// is used alanı olmalı ve false çekilince o kuponu o kullanıcı bi daha kullanılmamalı ya da tarihen gidilir

		public virtual Coupon Coupon { get; set; }
		public virtual AppUser User { get; set; }


	}

	// kupon sınıfında açıklaması yapıldı


}
	