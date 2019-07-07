namespace EasePrismDemos.Models
{
	public class Order : OrderSummary
	{
		public OrderProductSummary[] Products { get; set; }
	}
}
