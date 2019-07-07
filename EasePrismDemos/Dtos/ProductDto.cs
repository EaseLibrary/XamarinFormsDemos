using System.Collections.Generic;
using System.Text;

namespace EasePrismDemos.Dtos
{
	public class ProductDto : ProductSummaryDto
	{
		public string Description { get; set; }
		public string[] ImageUrls { get; set; }
	}
}
