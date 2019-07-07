using System.Collections.Generic;
using System.Text;

namespace EasePrismDemos.Models
{
	public class Product : ProductSummary
	{
		public string Description { get; set; }
		public string[] ImageUrls { get; set; }

	}
}
