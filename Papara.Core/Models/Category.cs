using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Core.Models
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Tags { get; set; }

		public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
	}
}
