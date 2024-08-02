namespace Papara.Core.Models
{
	public class DigitalWallet : BaseEntity
	{
		public string UserId { get; set; } 
		public AppUser User { get; set; }
		public decimal Balance { get; set; } // Cüzdan bakiyesi
		public decimal Points { get; set; } // Toplanan puanlar
	}
}


