namespace Papara.Core.DTOs.Request
{
	public class DigitalWalletRequestDTO : BaseRequestDTO
	{
		public decimal Balance { get; set; }  // Cüzdan bakiyesi
		public decimal Points { get; set; }
		public string UserId { get; set; }

	}


}
