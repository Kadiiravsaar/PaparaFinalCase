namespace Papara.Core.DTOs.Response
{
	public class OrderDetailWithDetailResponseDTO : BaseResponseDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string ProductName { get; set; } // Ürünün ismi
		public string ProductDescription { get; set; } // Ürünün açıklaması
		public decimal ProductPrice { get; set; } // Ürünün fiyatı
	}
}
