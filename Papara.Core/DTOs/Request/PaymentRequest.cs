namespace Papara.Core.DTOs.Request
{
	public class PaymentRequest
	{
		public string CardNumber { get; set; }
		public string Cvv { get; set; }
		public DateTime ExpiryDate { get; set; }
		public decimal Amount { get; set; }
	}
}
