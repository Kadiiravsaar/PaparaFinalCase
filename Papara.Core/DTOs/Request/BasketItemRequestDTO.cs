using Microsoft.AspNetCore.DataProtection;

namespace Papara.Core.DTOs.Request
{
	public class BasketItemRequestDTO
	{
		public int BasketId { get; set; }  // Kullanıcının ürün ekleyeceği sepetin ID'si
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
