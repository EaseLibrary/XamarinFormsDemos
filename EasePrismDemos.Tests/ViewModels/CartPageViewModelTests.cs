using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using EasePrismDemos.ViewModels;
using Moq;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;
using System.Linq;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.ViewModels
{
	public class CartPageViewModelTests : ViewModelTestBase
	{
		public CartPageViewModelTests()
		{
			RegisterType<CartPageViewModel>();
		}

		[Test]
		public async Task ICartRepositoryGetProductsIsCalledOnNavigation()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();
			GetMock<ICartRepository>().Verify(m => m.GetProducts(), Times.Once);
		}

		[Test]
		public async Task CartProductsIsPopulatedFromRepoOnNavigation()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			Assert.IsNotEmpty(vm.CartProducts);
		}

		[Test]
		public async Task ICartRepositoryUpdateProductsIsCalledForEachCartProductOnUpdateCartCommandExecute()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			vm.UpdateCartCommand.Execute();

			GetMock<ICartRepository>().Verify(m => m.UpdateProduct(It.IsAny<CartProduct>()), Times.Exactly(vm.CartProducts.Count()));
		}

		[Test]
		public async Task ICartRepositoryUpdateProductIsCalledForCartProductOnRemoveCartProductCommandExecute()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			var cartitem = vm.CartProducts.First();
			vm.RemoveCartProductCommand.Execute(cartitem);

			GetMock<ICartRepository>().Verify(m => m.UpdateProduct(cartitem), Times.Once);
		}

		[Test]
		public async Task ICartRepositoryUpdateProductIsCalledWithZeroQuantityOnRemoveCartProductCommandExecute()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			var cartitem = vm.CartProducts.First();
			vm.RemoveCartProductCommand.Execute(cartitem);

			GetMock<ICartRepository>().Verify(m => m.UpdateProduct(It.Is<CartProduct>(cp => cp.Quantity == 0)), Times.Once);
		}

		[Test]
		public async Task CartProductIsRemovedFromCartProductsOnRemoveCartProductCommandExecute()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			var cartitem = vm.CartProducts.First();
			vm.RemoveCartProductCommand.Execute(cartitem);

			Assert.IsFalse(vm.CartProducts.Any(cp => cp.Id == cartitem.Id));
		}

		[Test]
		public async Task CartProductsIsEmptyAfterClearCartProductCommandExecute()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			vm.ClearCartCommand.Execute();

			Assert.IsEmpty(vm.CartProducts);
		}

		[Test]
		public async Task AlertDialogIsNotDisplayedWhenValidOrderReturnsFromIOrderRepositorySubmitOrder()
		{
			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			vm.SubmitOrderFromCartCommand.Execute();

			ValidateMock<IPageDialogService>(mock =>
			{
				mock.Verify(s => s.DisplayAlertAsync("Order Error", "We were unable to process your order", "Ok"), Times.Never);
			});
		}

		[Test]
		public async Task AlertDialogIsDisplayedWhenNoOrderReturnsFromIOrderRepositorySubmitOrder()
		{
			onIOrderRepositoryMockCreated += mock => 
			{
				mock.Setup(s => s.SubmitOrder(It.IsAny<OrderProductRequest[]>()))
					.Returns<OrderProductRequest[]>(request => Task.FromResult<Order>(null));
			};

			var vm = await ResolveAndCallInitializeAsync<CartPageViewModel>();

			vm.SubmitOrderFromCartCommand.Execute();

			ValidateMock<IPageDialogService>(mock => 
			{
				mock.Verify(s => s.DisplayAlertAsync("Order Error", "We were unable to process your order", "Ok"), Times.Once);
			});
		}

	}
}
