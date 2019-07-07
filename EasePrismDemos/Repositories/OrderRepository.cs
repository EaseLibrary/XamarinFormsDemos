using EasePrismDemos.Dtos;
using EasePrismDemos.Models;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private AutoMapper.IMapper Mapper { get; }
		private Services.IOrderService OrderService { get; }

		public OrderRepository(AutoMapper.IMapper mapper, Services.IOrderService orderService)
		{
			Mapper = mapper;
			OrderService = orderService;
		}
		public async Task<Order> SubmitOrder(OrderProductRequest[] products)
		{
			var productsDto = Mapper.Map<OrderProductRequestDto[]>(products);
			var orderDto = await OrderService.SubmitOrder(productsDto);
			var order = Mapper.Map<Order>(orderDto);
			return order;
		}

		public async Task<OrderSummary[]> GetOrders()
		{
			var ordersDto = await OrderService.GetOrders();
			var orders = Mapper.Map<OrderSummary[]>(ordersDto);
			return orders;
		}

		public async Task<Order> GetOrder(int orderId)
		{
			var orderDto = await OrderService.GetOrder(orderId);
			var order = Mapper.Map<Order>(orderDto);
			return order;
		}
	}
}
