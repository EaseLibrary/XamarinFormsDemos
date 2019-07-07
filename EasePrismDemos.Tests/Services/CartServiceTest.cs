using EasePrismDemos.Dtos;
using EasePrismDemos.Services;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Services
{
	public class CartServiceTest : ServiceTestBase
	{
		public CartServiceTest()
		{
			RegisterType<CartService>();
		}

		[Test]
		public async Task UpdateProductReturnsAddedCartProductFromApi()
		{
			var cartService = ResolveType<CartService>();
			var newProduct = new CartProductDto
			{
				Id = 1,
				Quantity = 2
			};
			var product = await cartService.UpdateProduct(newProduct);
			Assert.AreEqual(newProduct.Quantity, product.Quantity);
			Assert.AreEqual(newProduct.Id, product.Id);
		}

		[Test]
		public async Task GetProductsReturnsCartProductsInCartFromApi()
		{
			var cartService = ResolveType<CartService>();
			var products = await cartService.GetProducts();
			Assert.AreEqual(1, products.Count());
		}

		[Test]
		public async Task ClearAllProductsReturnsEmptyCartProductsFromApi()
		{
			var cartService = ResolveType<CartService>();
			var products = await cartService.ClearAllProducts();
			Assert.AreEqual(0, products.Count());
		}
	}
}
