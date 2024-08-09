using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Exceptions
{
	public class DeleteRestrictionException : Exception
	{
		public DeleteRestrictionException() : base() { }

		public DeleteRestrictionException(string message) : base(message) { }

		public DeleteRestrictionException(string message, Exception innerException) : base(message, innerException) { }
	}
}
