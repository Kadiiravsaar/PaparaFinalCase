using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.UnitOfWorks
{
	public interface IUnitOfWork
	{
		Task CompleteAsync();	
		Task CompleteWithTransaction();
	}
}
