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
	public class CartRepositoryTest : AppTestBase
	{
		private Action<Mock<ICartService>> onMockICartServiceCreated;

		public CartRepositoryTest()
		{
			RegisterMockType(() => onMockICartServiceCreated);
			RegisterType<CartRepository>();
			RegisterType<TestApiData>();

			RegisterPerTestSetup(() =>
			{
				onMockICartServiceCreated = ICartServiceDefaultSetup;
			});
		}

		private void ICartServiceDefaultSetup(Mock<ICartService> serviceMock)
		{
			serviceMock.Setup(s => s.GetProducts())
				.Returns(() => Task.FromResult(ResolveType<TestApiData>().Cart.Cast<CartProductDto>().ToArray()));

			serviceMock.Setup(s => s.UpdateProduct(It.IsAny<CartProductDto>()))
				.Returns<CartProductDto>(request => Task.FromResult(request));

			serviceMock.Setup(s => s.ClearAllProducts())
				.Returns(() => Task.FromResult(new CartProductDto[0]));
		}

		[Test]
		public async Task GetProductsReturnsCartProductsFromService()
		{
			var repo = ResolveType<CartRepository>();
			var cartProducts = await repo.GetProducts();
			Assert.NotNull(cartProducts);
		}

		[Test]
		public async Task GetProductsCallsCartServiceGetProducts()
		{
			var repo = ResolveType<CartRepository>();
			_ = await repo.GetProducts();
			GetMock<ICartService>().Verify(s => s.GetProducts(), Times.Once);
		}

		[Test]
		public async Task UpdateProductReturnsCartProductFromService()
		{
			var repo = ResolveType<CartRepository>();
			var cartProduct = new CartProduct()
			{
				Id = 1,
				Name = "",
				Quantity = 3,
				Price = 0
			};
			var result = await repo.UpdateProduct(cartProduct);
			Assert.NotNull(result);
		}

		[Test]
		public async Task UpdateProductCallsCartServiceUpdateProduct()
		{
			var repo = ResolveType<CartRepository>();
			_ = await repo.UpdateProduct(null);
			GetMock<ICartService>().Verify(s => s.UpdateProduct(It.IsAny<CartProductDto>()), Times.Once);
		}

		[Test]
		public async Task ClearAllProductsReturnsEmptyCartProductsFromService()
		{
			var repo = ResolveType<CartRepository>();
			var cartProducts = await repo.ClearAllProducts();
			Assert.NotNull(cartProducts);
		}

		[Test]
		public async Task ClearAllProductsCallsCartServiceClearAllProducts()
		{
			var repo = ResolveType<CartRepository>();
			_ = await repo.ClearAllProducts();
			GetMock<ICartService>().Verify(s => s.ClearAllProducts(), Times.Once);
		}
	}
}
