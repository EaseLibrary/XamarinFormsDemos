using EasePrismDemos.Dtos;
using System.Threading.Tasks;

namespace EasePrismDemos.Services
{
	public class OrderService : ApiServiceBase, IOrderService
	{
		public OrderService(IHttpMessageHandlerFactory messageHandlerFactory)
			: base(messageHandlerFactory)
		{
		}

		public async Task<OrderDto> SubmitOrder(OrderProductRequestDto[] productRequests)
		{
			var result = await ApiPostData<OrderDto>(ApiEndpoints.PostOrderUri(), productRequests);
			return result.Data;
		}

		public async Task<OrderSummaryDto[]> GetOrders()
		{
			var result = await ApiGetData<OrderSummaryDto[]>(ApiEndpoints.GetOrdersUri());
			return result.Data;
		}

		public async Task<OrderDto> GetOrder(int orderId)
		{
			var result = await ApiGetData<OrderDto>(ApiEndpoints.GetOrderUri(orderId));
			return result.Data;
		}

	}
}
