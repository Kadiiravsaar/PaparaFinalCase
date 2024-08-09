using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.DTOs.Request
{
	public class UpdateUserRequestDTO
	{
		public string UserName { get; set; } // Kullanıcı adını güncellemek isterseniz ekleyebilirsiniz.
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
