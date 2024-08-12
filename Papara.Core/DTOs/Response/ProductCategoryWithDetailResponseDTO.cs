namespace Papara.Core.DTOs.Response
{
	public class ProductCategoryWithDetailResponseDTO : BaseResponseDTO
	{
		public int ProductId { get; set; }
		public int CategoryId { get; set; }
		public string ProductName { get; set; }
		public string CategoryName { get; set; }
		public string ProductDescription { get; set; } // Ürün açıklaması
		public decimal ProductPrice { get; set; } // Ürün fiyatı
		public string CategoryUrl { get; set; } // Kategori URL'i
		public string CategoryTags { get; set; } // Kategori etiketleri
	}
}
