namespace Papara.Core.DTOs.Response
{
	public class BasketResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }	
		public List<BasketItemResponseDTO> Items { get; set; }
	}


	public class BasketWithDetailResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public decimal TotalAmount { get; set; } // Sepetteki tüm ürünlerin toplam tutarı
		public decimal DiscountAmount { get; set; } // Uygulanan indirim miktarı
		public decimal FinalPrice { get; set; } // İndirim sonrası fiyat
		public decimal? PointsEarned { get; set; } // Bu siparişten kazanılacak puan
		//public decimal? PointUsed { get; set; } // Önceki siparişlerden kullanılacak puanlar

		public List<BasketItemResponseDTO> Items { get; set; }
	}

}
