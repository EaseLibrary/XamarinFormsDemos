using System;

namespace EasePrismDemos.Dtos
{
	public class OrderSummaryDto
	{
		public int Id { get; set; }
		public decimal Total { get; set; }
		public DateTime OrderPlaced { get; set; }
	}
}
