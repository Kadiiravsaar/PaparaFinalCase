namespace Papara.Core.DTOs.Response
{
	public class BasketItemResponseDTO : BaseResponseDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public decimal PointsEarned { get; set; } // Kazanılan puanlar için yeni özellik
		public decimal PointsPercentage { get; set; } // Kazanılan puanlar için yeni özellik
		public decimal MaxPoint { get; set; } // Kazanılan puanlar için yeni özellik
	}
}
