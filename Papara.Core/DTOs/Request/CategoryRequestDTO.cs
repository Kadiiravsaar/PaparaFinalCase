namespace Papara.Core.DTOs.Request
{
	public class CategoryRequestDTO : BaseRequestDTO
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Tags { get; set; }

		public List<int>? ProductIds { get; set; } // Category Id'leri içeren liste

	}

}
