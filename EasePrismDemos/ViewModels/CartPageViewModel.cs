using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EasePrismDemos.ViewModels
{
	public class CartPageViewModel : ViewModelBase
	{
		private ObservableCollection<CartProduct> cartProducts;

		private ICartRepository CartRepository { get; }
		private IOrderRepository OrderRepository { get; }
		private IPageDialogService PageDialogService { get; }

		public ObservableCollection<CartProduct> CartProducts
		{
			get => cartProducts;
			private set => SetProperty(ref cartProducts, value);
		}

		public DelegateCommand ClearCartCommand => new DelegateCommand(async () => await ClearCartCommandExecute());

		public DelegateCommand UpdateCartCommand => new DelegateCommand(async () => await UpdateCartCommandExecute());

		public DelegateCommand<CartProduct> RemoveCartProductCommand => new DelegateCommand<CartProduct>(async cp => await RemoveCartProductCommandExecute(cp));

		public DelegateCommand SubmitOrderFromCartCommand => new DelegateCommand(async () => await SubmitOrderFromCartCommandExecute());

		public CartPageViewModel(
			ICartRepository cartRepository,
			IOrderRepository orderRepository,
			IPageDialogService pageDialogService,
			INavigationService navigationService)
			: base(navigationService)
		{
			CartRepository = cartRepository;
			OrderRepository = orderRepository;
			PageDialogService = pageDialogService;
		}

		private async Task ClearCartCommandExecute()
		{
			var cart = await CartRepository.ClearAllProducts();
			CartProducts = new ObservableCollection<CartProduct>(cart);
		}

		private async Task UpdateCartCommandExecute()
		{
			foreach (var product in CartProducts)
			{
				_ = await CartRepository.UpdateProduct(product);
			}
		}

		private async Task RemoveCartProductCommandExecute(CartProduct cp)
		{
			cp.Quantity = 0;
			var result = await CartRepository.UpdateProduct(cp);
			if (result.Quantity == 0) CartProducts.Remove(cp);
		}

		private async Task SubmitOrderFromCartCommandExecute()
		{
			var orderRequest = CartProducts.Select(cp => new OrderProductRequest() { Id = cp.Id, Quantity = cp.Quantity });
			var order = await OrderRepository.SubmitOrder(orderRequest.ToArray());
			if (order == null)
			{
				await PageDialogService.DisplayAlertAsync("Order Error", "We were unable to process your order", "Ok");
			}
		}

		public override async Task OnNavigatedToAsync(INavigationParameters parameters)
		{
			if (parameters.GetNavigationMode() == NavigationMode.Back) return;

			var repoProducts = await CartRepository.GetProducts();
			CartProducts = new ObservableCollection<CartProduct>(repoProducts);
		}
	}


}
