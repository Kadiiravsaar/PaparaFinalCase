namespace Papara.Core.DTOs.Response
{
	public class OrderResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal? PointUsed { get; set; }


		public List<OrderDetailResponseDTO> OrderDetails { get; set; }
	}


	public class OrderWithDetailResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public string UserName { get; set; }  // Kullanıcı adını da ekleyelim
		public decimal? PointUsed { get; set; }

		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		public List<OrderDetailResponseDTO> OrderDetails { get; set; }

	}

	
}
