namespace Papara.Core.DTOs.Response
{
	public class BasketWithDetailResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public decimal TotalAmount { get; set; } // Sepetteki tüm ürünlerin toplam tutarı
		public decimal DiscountAmount { get; set; } // Uygulanan indirim miktarı
		public decimal FinalPrice { get; set; } // İndirim sonrası fiyat
		public decimal? PointsEarned { get; set; } // Bu siparişten kazanılacak puan
		public List<BasketItemResponseDTO> Items { get; set; }
	}

}
