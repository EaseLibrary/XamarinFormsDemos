using EasePrismDemos.Dtos;
using EasePrismDemos.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Services
{
	public class OrderServiceTests : ServiceTestBase
	{
		public OrderServiceTests()
		{
			RegisterType<OrderService>();
		}

		[Test]
		public async Task GetOrdersReturnsAllOrdersFromApi()
		{
			var orderService = ResolveType<OrderService>();
			var orders = await orderService.GetOrders();
			Assert.AreEqual(1, orders.Length);
		}

		[Test]
		public async Task GetOrdersReturnsNonNullFromApi()
		{
			var orderService = ResolveType<OrderService>();
			var orders = await orderService.GetOrders();
			Assert.IsNotNull(orders);
		}

		[Test]
		public async Task GetOrderReturnsNonNullFromApiWhenOrderIdExists()
		{
			var orderService = ResolveType<OrderService>();
			var order = await orderService.GetOrder(1);
			Assert.IsNotNull(order);
		}

		[Test]
		public async Task GetOrderReturnsNullFromApiWhenOrderIdDoesNotExist()
		{
			var orderService = ResolveType<OrderService>();
			var order = await orderService.GetOrder(2);
			Assert.IsNull(order);
		}

		[Test]
		public async Task SubmitOrdersReturnsNewOrderFromApi()
		{
			var orderService = ResolveType<OrderService>();
			var newOrder = new OrderProductRequestDto[]
			{
				new OrderProductRequestDto { Id = 1, Quantity = 1}
			};
			var order = await orderService.SubmitOrder(newOrder);
			Assert.AreEqual(newOrder[0].Quantity, order.Products[0].Quantity);
			Assert.AreEqual(newOrder[0].Id, order.Products[0].Id);
		}
	}
}
