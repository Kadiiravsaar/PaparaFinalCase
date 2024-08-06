using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Exceptions
{
	public class ClientSideException : Exception
	{
		public ClientSideException() { }

		public ClientSideException(string? message)
			: base(message) { }

		public ClientSideException(string? message, Exception? exception)
			: base(message, exception) { }
	}
}
