using Papara.Core.Models;

namespace Papara.Core.DTOs.Request
{
	public class CouponUsageRequestDTO
	{
		public int CouponId { get; set; }
		public string UserId { get; set; }
		public int BasketId { get; set; }
		public DateTime UsedDate { get; set; }
	}	


}
