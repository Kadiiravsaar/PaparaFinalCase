namespace Papara.Core.DTOs.Response
{
	public class ProductWithDetailResponseDTO : BaseResponseDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public decimal PointsPercentage { get; set; }
		public decimal MaxPoints { get; set; }
        public  int Stock { get; set; }
		public List<CategoryResponseDTO>? Categories { get; set; }

	}

}
