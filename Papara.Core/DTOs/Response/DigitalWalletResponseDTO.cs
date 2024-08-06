namespace Papara.Core.DTOs.Response
{
	public class DigitalWalletResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public decimal Balance { get; set; }
		public decimal Points { get; set; }
	}


	public class DigitalWalletWithDetailResponseDTO : BaseResponseDTO
	{
		public string UserId { get; set; }
		public decimal Balance { get; set; }
		public decimal Points { get; set; }
		public string UserName { get; set; } // Kullanıcının adını ekleyerek detaylandırabiliriz
	}


	
}
