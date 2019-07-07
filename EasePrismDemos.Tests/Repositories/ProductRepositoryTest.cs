using Ease.NUnit.DryIoc.PrismForms;
using EasePrismDemos.Dtos;
using EasePrismDemos.Repositories;
using EasePrismDemos.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Repositories
{
	public class ProductRepositoryTest : AppTestBase
	{
		private Action<Mock<IProductService>> onMockIProductServiceCreated;

		public ProductRepositoryTest()
		{
			RegisterMockType(() => onMockIProductServiceCreated);
			RegisterType<ProductRepository>();
			RegisterType<TestApiData>();

			RegisterPerTestSetup(() => 
			{
				onMockIProductServiceCreated = IProductServiceDefaultSetup;
			});
		}

		private void IProductServiceDefaultSetup(Mock<IProductService> productServiceMock)
		{
			productServiceMock.Setup(ps => ps.GetProducts())
				.Returns(() => Task.FromResult(ResolveType<TestApiData>().Products.Cast<ProductSummaryDto>().ToArray()));

			productServiceMock.Setup(ps => ps.GetProduct(It.IsAny<int>()))
				.Returns<int>(productId => Task.FromResult(ResolveType<TestApiData>().Products.FirstOrDefault(p => p.Id == productId)));

		}

		[Test]
		public async Task GetProductsReturnsProductsFromService()
		{
			var repo = ResolveType<ProductRepository>();
			var products = await repo.GetProducts();
			Assert.NotNull(products);
		}

		[Test]
		public async Task GetProductReturnsProductFromServiceWhenValidProductId()
		{
			var repo = ResolveType<ProductRepository>();
			var product = await repo.GetProduct(1);
			Assert.NotNull(product);
		}
	}
}
