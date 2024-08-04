using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.Models
{
	public class Stock : BaseEntity
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		// Navigation property
		public Product Product { get; set; }
	}
}
