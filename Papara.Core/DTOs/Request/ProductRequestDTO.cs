namespace Papara.Core.DTOs.Request
{
	public class ProductRequestDTO : BaseRequestDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public decimal PointsPercentage { get; set; }  // Üründen kazanılacak puan yüzdesi
		public decimal MaxPoint { get; set; }         // Kazanılacak maksimum puan
		public List<int> CategoryIds { get; set; } // Category Id'leri içeren liste
	}
}
