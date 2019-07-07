using EasePrismDemos.Repositories;
using EasePrismDemos.ViewModels;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.ViewModels
{
	public class ProductsPageViewModelTests : ViewModelTestBase
	{
		public ProductsPageViewModelTests()
		{
			RegisterType<ProductsPageViewModel>();
		}

		[Test]
		public async Task IProductRepositoryGetProductsIsCalledOnNavigationWhenModeIsNew()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.New);
			GetMock<IProductRepository>().Verify(m => m.GetProducts(), Times.Once);
		}

		[Test]
		public async Task IProductRepositoryGetProductsIsCalledOnNavigationWhenModeIsForward()
		{
			var vm = ResolveType<ProductsPageViewModel>();
			await vm.OnNavigatedToAsync(CreateNavigationParameters(NavigationMode.Forward));
			GetMock<IProductRepository>().Verify(m => m.GetProducts(), Times.Once);
		}

		[Test]
		public async Task IProductRepositoryGetProductsIsCalledOnNavigationWhenModeIsRefresh()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.Refresh);
			GetMock<IProductRepository>().Verify(m => m.GetProducts(), Times.Once);
		}

		[Test]
		public async Task IProductRepositoryGetProductsIsNotCalledOnNavigationWhenModeIsBack()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.Back);
			GetMock<IProductRepository>().Verify(m => m.GetProducts(), Times.Never);
		}

		[Test]
		public async Task ProductsAreLoadedFromRepositoryOnNavigatedTo()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.New);
			Assert.IsNotEmpty(vm.Products);
		}
		
		[Test]
		public async Task ProductDetailPageIsNavigatedToOnViewProductDetailsCommandExecute()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.New);
			var firstProduct = vm.Products.First();
			vm.ViewProductDetailsCommand.Execute(firstProduct);
			VerifyNavigation("ProductDetailPage", p => true, Times.Once);
		}
		
		[Test]
		public async Task ProductDetailPageIsNavigatedToWithProductIdOnViewProductDetailsCommandExecute()
		{
			var vm = await ResolveAndCallOnNavigatedToAsync<ProductsPageViewModel>(NavigationMode.New);
			var firstProduct = vm.Products.First();
			vm.ViewProductDetailsCommand.Execute(firstProduct);
			VerifyNavigation(
				"ProductDetailPage", 
				p => p.ContainsKey("productId") && p.GetValue<int>("productId").Equals(firstProduct.Id), 
				Times.Once);
		}

	}
}
