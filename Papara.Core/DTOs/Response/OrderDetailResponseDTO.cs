namespace Papara.Core.DTOs.Response
{
	public class OrderDetailResponseDTO : BaseResponseDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
