namespace Papara.Core.Models
{
	public class Order : BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal? PointUsed { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
	}
}


