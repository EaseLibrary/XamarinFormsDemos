using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using Moq;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.ViewModels
{
	public class ViewModelTestBase : AppTestBase
	{
		protected Action<Mock<ICartRepository>> onICartRepositoryMockCreated;
		protected Action<Mock<IOrderRepository>> onIOrderRepositoryMockCreated;
		protected Action<Mock<IProductRepository>> onIProductRepositoryMockCreated;

		public ViewModelTestBase()
		{
			RegisterMockType(() => onICartRepositoryMockCreated);
			RegisterMockType(() => onIOrderRepositoryMockCreated);
			RegisterMockType(() => onIProductRepositoryMockCreated);

			RegisterPerTestSetup(() =>
			{
				onICartRepositoryMockCreated = ICartRepositoryDefaultSetup;
				onIOrderRepositoryMockCreated = IOrderRepositoryDefaultSetup;
				onIProductRepositoryMockCreated = IProductRepositoryDefaultSetup;
			});
		}

		protected void ICartRepositoryDefaultSetup(Mock<ICartRepository> mock)
		{
			mock.Setup(r => r.ClearAllProducts())
				.Returns(Task.FromResult(new CartProduct[0]));

			mock.Setup(r => r.GetProducts())
				.Returns(Task.FromResult(
					Mapper().Map<Dtos.CartProductDto[], CartProduct[]>(
						ResolveType<TestApiData>().Cart.ToArray())));

			mock.Setup(r => r.UpdateProduct(It.IsAny<CartProduct>()))
				.Returns<CartProduct>(p => Task.FromResult(p));
		}
		
		protected void IOrderRepositoryDefaultSetup(Mock<IOrderRepository> mock)
		{
			mock.Setup(r => r.GetOrders())
				.Returns(Task.FromResult(
					Mapper().Map<Dtos.OrderSummaryDto[], OrderSummary[]>(
						ResolveType<TestApiData>().Orders.ToArray())));

			mock.Setup(r => r.GetOrder(It.IsAny<int>()))
				.Returns<int>(id => Task.FromResult(
					Mapper().Map<Dtos.OrderDto, Order>(
						ResolveType<TestApiData>().Orders.FirstOrDefault(o => o.Id == id))));

			mock.Setup(s => s.SubmitOrder(It.IsAny<OrderProductRequest[]>()))
				.Returns<OrderProductRequest[]>(request =>
				{
					var testData = ResolveType<TestApiData>();
					var orderDto = testData.CreateOrderDto(
						2,
						DateTime.Now,
						request.Select(r => testData.CreateOrderProductSummaryDto(r.Id, r.Quantity)).ToArray()
						);
					var order = Mapper().Map<Dtos.OrderDto, Order>(orderDto);
					return Task.FromResult(order);
				});
		}

		protected void IProductRepositoryDefaultSetup(Mock<IProductRepository> mock)
		{
			mock.Setup(r => r.GetProducts())
				.Returns(Task.FromResult(
					Mapper().Map<Dtos.ProductSummaryDto[], ProductSummary[]>(
						ResolveType<TestApiData>().Products.ToArray())));

			mock.Setup(r => r.GetProduct(It.IsAny<int>()))
				.Returns<int>(id => Task.FromResult(
					Mapper().Map<Dtos.ProductDto, Product>(
						ResolveType<TestApiData>().Products.FirstOrDefault(p => p.Id == id))));
		}

	}
}
