using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.DTOs.Response
{
	public class StockResponseDTO : BaseResponseDTO
	{
		public int Stock { get; set; }
	}

	public class StockWithDetailResponseDTO
	{
		public int Stock { get; set; }
	//	public DateTime UpdatedDate { get; set; }
		public ProductResponseDTO Product { get; set; }
	}

}
