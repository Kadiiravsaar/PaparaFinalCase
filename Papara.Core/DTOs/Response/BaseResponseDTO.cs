using Papara.Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.DTOs.Response
{
	public class BaseResponseDTO
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
	}


	public class NoContentDto
	{

	}
}
