namespace Papara.Core.DTOs.Response
{
	public class CategoryWithDetailResponseDTO : BaseResponseDTO
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Tags { get; set; }
		public List<ProductResponseDTO>? Products { get; set; }
	}
}
