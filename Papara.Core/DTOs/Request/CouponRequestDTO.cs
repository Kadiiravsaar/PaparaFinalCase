namespace Papara.Core.DTOs.Request
{
	public class CouponRequestDTO : BaseRequestDTO
	{
		public string CouponCode { get; set; }  // Benzersiz kupon kodu
		public decimal Amount { get; set; }  // Kuponun değeri
		public DateTime ExpiryDate { get; set; }  // Kuponun son kullanma tarihi
	}


}
