namespace EasePrismDemos.Dtos
{
	public class OrderDto : OrderSummaryDto
	{
		public OrderProductSummaryDto[] Products { get; set; }
	}
}
