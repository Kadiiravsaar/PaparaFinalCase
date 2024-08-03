namespace Papara.Core.Models
{
	public class Basket: BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }

		public decimal TotalPrice { get; set; } // Sepetteki tüm ürünlerin toplam tutarı

		public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
		
	}
}
