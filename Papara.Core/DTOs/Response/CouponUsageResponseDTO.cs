namespace Papara.Core.DTOs.Response
{
	public class CouponUsageResponseDTO : BaseResponseDTO
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		//public string CouponCode { get; set; } // Coupon kodu ekleyelim
		//public string UserName { get; set; } // Kullanıcı adı ekleyelim
	}


	public class CouponUsageWithDetailResponseDTO : BaseResponseDTO
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		public string CouponCode { get; set; } // Coupon kodu ekleyelim
		public string UserName { get; set; } // Kullanıcı adı ekleyelim

		// Ekstra detaylar
		public CouponResponseDTO Coupon { get; set; }
		public AppUserResponseDTO User { get; set; }
		public BasketResponseDTO Basket { get; set; }
	}
}
