using System.Runtime.ConstrainedExecution;

namespace Papara.Core.DTOs.Request
{
	public class OrderRequestDTO : BaseRequestDTO
	{
		public List<OrderDetailRequestDTO> OrderDetails { get; set; }

		 public string OrderNumber { get; set; } // order oluşurken rasgele sipariş numarası oluşturucaz
		//Her sipariş için bir sipariş numarası oluşturulmalı ve max 9 karakter uzunluğunda numerik olmalı.
		// arkada otomatik olarak yönetilmeli

		public decimal TotalAmount { get; set; }
		//public decimal? CouponAmount { get; set; }
		public decimal? PointUsed { get; set; }
	}
}
