namespace Papara.Core.Models
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public decimal PointsPercentage { get; set; } // Üründen  kazanılacak puan yüzdesi
		public decimal MaxPoints { get; set; } // Kazanılacak maksimum puan

		public virtual ICollection<ProductCategory> ProductCategories { get; set; }
	}
}
