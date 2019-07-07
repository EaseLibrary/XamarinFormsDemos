using EasePrismDemos.Models;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public interface IOrderRepository
	{
		Task<Order> GetOrder(int orderId);
		Task<OrderSummary[]> GetOrders();
		Task<Order> SubmitOrder(OrderProductRequest[] products);
	}
}