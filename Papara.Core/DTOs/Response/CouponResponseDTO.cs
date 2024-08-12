using Papara.Core.Models;

namespace Papara.Core.DTOs.Response
{
	public class CouponResponseDTO : BaseResponseDTO
	{
		public string CouponCode { get; set; }
		public decimal Amount { get; set; }
		public DateTime ExpiryDate { get; set; }
	}
}
