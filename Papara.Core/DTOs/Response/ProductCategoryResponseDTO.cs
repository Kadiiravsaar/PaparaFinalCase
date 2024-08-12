namespace Papara.Core.DTOs.Response
{
	public class ProductCategoryResponseDTO : BaseResponseDTO
	{
		public int ProductId { get; set; }
		public int CategoryId { get; set; }
		public string ProductName { get; set; }  // Ürün adı ekstra bilgi olarak eklenebilir
		public string CategoryName { get; set; }  // Kategori adı ekstra bilgi olarak eklenebilir
	}
}
