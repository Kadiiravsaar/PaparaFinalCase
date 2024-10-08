﻿namespace Papara.Core.DTOs.Response
{
	public class ProductResponseDTO : BaseResponseDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public decimal PointsPercentage { get; set; }
		public decimal MaxPoint { get; set; }

	}

}
