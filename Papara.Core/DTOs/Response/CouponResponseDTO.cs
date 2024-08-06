using Papara.Core.Models;

namespace Papara.Core.DTOs.Response
{
	public class CouponResponseDTO : BaseResponseDTO
	{
		public string CouponCode { get; set; }
		public decimal Amount { get; set; }
	
		public DateTime ExpiryDate { get; set; }
	}



	public class CouponWithDetailResponseDTO : BaseResponseDTO
	{
		public string CouponCode { get; set; }
		public decimal Amount { get; set; }
		public DateTime ExpiryDate { get; set; }
	
		public List<BasketResponseDTO> Baskets  { get; set; }  // Order nesnelerini içerecek
		public List<CouponUsageResponseDTO> Usages { get; set; }
	}



}
