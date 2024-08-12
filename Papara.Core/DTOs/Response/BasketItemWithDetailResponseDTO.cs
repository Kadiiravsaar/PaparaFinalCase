namespace Papara.Core.DTOs.Response
{
	public class BasketItemWithDetailResponseDTO : BaseResponseDTO
	{
		public int BasketItemId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public string UserId { get; set; }  // Basket'e ait kullanıcı ID'si
	}

}
