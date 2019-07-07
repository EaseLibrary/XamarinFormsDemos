using System.Threading.Tasks;
using EasePrismDemos.Dtos;

namespace EasePrismDemos.Services
{
	public interface IOrderService
	{
		Task<OrderDto> GetOrder(int orderId);
		Task<OrderSummaryDto[]> GetOrders();
		Task<OrderDto> SubmitOrder(OrderProductRequestDto[] productRequests);
	}
}