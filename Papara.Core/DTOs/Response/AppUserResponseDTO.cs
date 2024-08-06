namespace Papara.Core.DTOs.Response
{
	public class AppUserResponseDTO 
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public DateTime CreatedDate { get; set; }
	}
}
