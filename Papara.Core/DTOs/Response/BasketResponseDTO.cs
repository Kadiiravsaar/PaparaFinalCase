namespace Papara.Core.DTOs.Response
{
	public class BasketResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }	
		public List<BasketItemResponseDTO> Items { get; set; }
	}

}
