namespace Papara.Core.DTOs.Request
{
	public class EmptyBasketRequestDTO : BaseRequestDTO
	{
	}


	public class BasketRequestDTO : BaseRequestDTO
	{
		public int? CouponId { get; set; }
	}
}
