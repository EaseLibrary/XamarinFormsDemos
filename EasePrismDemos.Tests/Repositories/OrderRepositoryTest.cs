using EasePrismDemos.Dtos;
using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using EasePrismDemos.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Repositories
{
	public class OrderRepositoryTest : AppTestBase
	{
		private Action<Mock<IOrderService>> onMockIOrderServiceCreated;

		public OrderRepositoryTest()
		{
			RegisterMockType(() => onMockIOrderServiceCreated);
			RegisterType<OrderRepository>();
			RegisterType<TestApiData>();

			RegisterPerTestSetup(() =>
			{
				onMockIOrderServiceCreated = IOrderServiceDefaultSetup;
			});
		}

		private void IOrderServiceDefaultSetup(Mock<IOrderService> serviceMock)
		{
			serviceMock.Setup(s => s.GetOrders())
				.Returns(() => Task.FromResult(ResolveType<TestApiData>().Orders.Cast<OrderSummaryDto>().ToArray()));

			serviceMock.Setup(s => s.GetOrder(It.IsAny<int>()))
				.Returns<int>(id => Task.FromResult(ResolveType<TestApiData>().Orders.FirstOrDefault(x => x.Id == id)));

			serviceMock.Setup(s => s.SubmitOrder(It.IsAny<OrderProductRequestDto[]>()))
				.Returns<OrderProductRequestDto[]>(request =>
				{
					var testData = ResolveType<TestApiData>();
					var order = testData.CreateOrderDto(
						2, 
						DateTime.Now,
						request.Select(r => testData.CreateOrderProductSummaryDto(r.Id, r.Quantity)).ToArray()
						);
					return Task.FromResult(order);
				});
		}

		[Test]
		public async Task GetOrdersReturnsOrdersFromService()
		{
			var repo = ResolveType<OrderRepository>();
			var orders = await repo.GetOrders();
			Assert.NotNull(orders);
		}

		[Test]
		public async Task GetOrdersCallsOrderServiceGetOrders()
		{
			var repo = ResolveType<OrderRepository>();
			_ = await repo.GetOrders();
			GetMock<IOrderService>().Verify(s => s.GetOrders(), Times.Once);
		}

		[Test]
		public async Task GetOrderReturnsOrderFromServiceWhenValidOrderId()
		{
			var repo = ResolveType<OrderRepository>();
			var order = await repo.GetOrder(1);
			Assert.NotNull(order);
		}

		[Test]
		public async Task GetOrderCallsOrderServiceGetOrder()
		{
			var repo = ResolveType<OrderRepository>();
			_ = await repo.GetOrder(1);
			GetMock<IOrderService>().Verify(s => s.GetOrder(It.IsAny<int>()), Times.Once);
		}

		[Test]
		public async Task SubmitOrderReturnsOrderFromApi()
		{
			var repo = ResolveType<OrderRepository>();
			var orderProducts = new OrderProductRequest[]
			{
				new OrderProductRequest
				{
					Id = 1,
					Quantity = 1
				}
			};

			var order = await repo.SubmitOrder(orderProducts);
			Assert.NotNull(order);
		}

		[Test]
		public async Task SubmitOrderCallsOrderServiceSubmitOrderOnce()
		{
			var repo = ResolveType<OrderRepository>();
			var orderProducts = new OrderProductRequest[]
			{
				new OrderProductRequest
				{
					Id = 1,
					Quantity = 1
				}
			};

			var order = await repo.SubmitOrder(orderProducts);
			GetMock<IOrderService>().Verify(s => s.SubmitOrder(It.IsAny<OrderProductRequestDto[]>()), Times.Once);
		}

	}
}
