using EasePrismDemos.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Services
{
	public class ProductServiceTests : ServiceTestBase
	{
		public ProductServiceTests()
		{
			RegisterType<ProductService>();
		}

		[Test]
		public async Task GetProductsReturnsAllProductsFromApi()
		{
			var productService = ResolveType<ProductService>();
			var products = await productService.GetProducts();
			Assert.AreEqual(3, products.Length);
		}

		[Test]
		public async Task GetProductsReturnsNonNullFromApi()
		{
			var productService = ResolveType<ProductService>();
			var products = await productService.GetProducts();
			Assert.IsNotNull(products);
		}

		[Test]
		public async Task GetExistingProductReturnsNonNullFromApi()
		{
			var productService = ResolveType<ProductService>();
			var product = await productService.GetProduct(1);
			Assert.IsNotNull(product);
		}

		[Test]
		public async Task GetNonExistingProductReturnsNullFromApi()
		{
			var productService = ResolveType<ProductService>();
			var product = await productService.GetProduct(4);
			Assert.IsNull(product);
		}
	}
}
