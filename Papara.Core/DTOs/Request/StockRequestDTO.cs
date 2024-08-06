using Papara.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.DTOs.Request
{
	public class StockRequestDTO
	{
		public int ProductId { get; set; }
		public int Stock { get; set; }
		//public DateTime UpdatedAt { get; set; }
	}
}
