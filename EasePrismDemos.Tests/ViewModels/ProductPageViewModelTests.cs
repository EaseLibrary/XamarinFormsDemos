using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using EasePrismDemos.ViewModels;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.ViewModels
{
	public class ProductPageViewModelTests : ViewModelTestBase
	{
		public ProductPageViewModelTests()
		{
			RegisterType<ProductDetailPageViewModel>();
		}

		[Test]
		public async Task IProductRepositoryGetProductIsCalledOnNavigation()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New, 
				new KeyValuePair<string, object>("productId", 1));
			GetMock<IProductRepository>().Verify(m => m.GetProduct(1), Times.Once);
		}

		[Test]
		public async Task ProductIsLoadedFromRepositoryOnNavigatedTo()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			Assert.IsNotNull(vm.Product);
		}

		[Test]
		public async Task NavigationServiceGoBackIsCalledFromReturnToProductPageCommandExecute()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			vm.ReturnToProductsPageCommand.Execute();
			VerifyNavigationGoBackAsync(Times.Once);
		}

		[Test]
		public async Task AddToCartCommandCannotBeExecutedWithoutQuantity()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			vm.Quantity = 0;
			var actual = vm.AddToCartCommand.CanExecute();
			Assert.AreEqual(false, actual);
		}

		[Test]
		public async Task AddToCartCommandCanBeExecutedWithQuantity()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			vm.Quantity = 1;
			var actual = vm.AddToCartCommand.CanExecute();
			Assert.AreEqual(true, actual);
		}

		[Test]
		public async Task AddToCartCommandCallsCartRepositoryUpdateProduct()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			vm.Quantity = 1;
			vm.AddToCartCommand.Execute();
			ValidateMock<ICartRepository>(mock => 
				mock.Verify(r => r.UpdateProduct(It.IsAny<CartProduct>())));
		}

		[Test]
		public async Task AddToCartCommandCallsCartRepositoryUpdateProductWithProductId()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			var productId = vm.Product.Id;
			vm.Quantity = 1;
			vm.AddToCartCommand.Execute();
			ValidateMock<ICartRepository>(mock =>
				mock.Verify(r => r.UpdateProduct(It.Is<CartProduct>(cp => cp.Id == productId))));
		}

		[Test]
		public async Task AddToCartCommandCallsCartRepositoryUpdateProductWithQuantity()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));
			var productId = vm.Product.Id;
			vm.Quantity = 10;
			vm.AddToCartCommand.Execute();
			ValidateMock<ICartRepository>(mock =>
				mock.Verify(r => r.UpdateProduct(It.Is<CartProduct>(cp => cp.Quantity == vm.Quantity))));
		}

		[Test]
		public async Task DialogServiceDislayAlertAsyncIsCalledWhenCartRepositoryUpdateProductFailsOnToCartCommand()
		{
			onICartRepositoryMockCreated += mock =>
			{
				mock.Setup(s => s.UpdateProduct(It.IsAny<CartProduct>()))
					.Returns<CartProduct>(request => Task.FromResult<CartProduct>(null));
			};

			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));

			vm.Quantity = 10;
			vm.AddToCartCommand.Execute();

			ValidateMock<IPageDialogService>(mock =>
			{
				mock.Verify(s => s.DisplayAlertAsync("Cart Error", "We were unable to add the item to your cart", "Ok"), Times.Once);
			});
		}

		[Test]
		public async Task DialogServiceDislayAlertAsyncIsNotCalledWhenCartRepositoryUpdateProductSucceedsOnToCartCommand()
		{

			var vm = await ResolveAndCallOnNavigatedToAsync<ProductDetailPageViewModel>(
				NavigationMode.New,
				new KeyValuePair<string, object>("productId", 1));

			vm.Quantity = 10;
			vm.AddToCartCommand.Execute();

			ValidateMock<IPageDialogService>(mock =>
			{
				mock.Verify(s => s.DisplayAlertAsync("Cart Error", "We were unable to add the item to your cart", "Ok"), Times.Never);
			});
		}

	}
}
