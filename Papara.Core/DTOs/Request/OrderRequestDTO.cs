using System.Runtime.ConstrainedExecution;

namespace Papara.Core.DTOs.Request
{
	public class OrderRequestDTO : BaseRequestDTO
	{
		public List<OrderDetailRequestDTO> OrderDetails { get; set; }

		 public string OrderNumber { get; set; } // order oluşurken rasgele sipariş numarası oluşturucaz
		public decimal TotalAmount { get; set; }
		public decimal? PointUsed { get; set; }
	}

}
