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
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public DateTime CreatedDate { get; set; }


		// Kullanıcının siparişleriyle ilgili koleksiyon
		public virtual ICollection<Order> Orders { get; set; }

		// Kullanıcının alışveriş sepeti
		public virtual ICollection<Basket> Baskets { get; set; }

		// kullanıcının yalnızca bir aktif alışveriş sepetine sahip olabileceği anlamına gelir.

		// Kullanıcının dijital cüzdanı (para ve puan işlemleri için)
		public virtual DigitalWallet DigitalWallet { get; set; }
	}
}
