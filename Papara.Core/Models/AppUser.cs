using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.Models
{
	public class AppUser : IdentityUser
	{
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiryTime { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public DateTime CreatedDate { get; set; }


		// Kullanıcının dijital cüzdanındaki para miktarı
		public decimal DigitalWalletBalance { get; set; }

		//// Kullanıcının biriktirdiği puan miktarı
		public decimal PointsBalance { get; set; }


		// Kullanıcının siparişleriyle ilgili koleksiyon
		public virtual ICollection<Order> Orders { get; set; }

		// Kullanıcının alışveriş sepeti
		public virtual Basket Basket { get; set; }
		// kullanıcının yalnızca bir aktif alışveriş sepetine sahip olabileceği anlamına gelir.

		// Kullanıcının dijital cüzdanı (para ve puan işlemleri için)
		public virtual DigitalWallet DigitalWallet { get; set; }
	}
}
