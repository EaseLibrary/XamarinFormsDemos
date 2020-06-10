using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace EasePrismDemos.ViewModels
{
	public class ProductDetailPageViewModel : ViewModelBase
	{
		private IProductRepository ProductRepository { get; }
		private ICartRepository CartRepository { get; }
		private IPageDialogService PageDialogService { get; }

		private ProductSummary _product;
		public ProductSummary Product
		{
			get => _product;
			set => SetProperty(ref _product, value);
		}

		private int _quantity;

		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}
		
		public DelegateCommand AddToCartCommand =>
			new DelegateCommand(
				async () => await AddToCartCommandExecute(),
				() => Quantity > 0).ObservesProperty(() => Quantity);


		public DelegateCommand ReturnToProductsPageCommand =>
			new DelegateCommand(async () => await NavigationService.GoBackAsync());

		public ProductDetailPageViewModel(
			IProductRepository productRepository, 
			ICartRepository cartRepository,
			IPageDialogService pageDialogService,
			INavigationService navigationService) 
			: base(navigationService)
		{
			ProductRepository = productRepository;
			CartRepository = cartRepository;
			PageDialogService = pageDialogService;
		}

		public override async Task InitializeAsync(INavigationParameters parameters)
		{
			var id = parameters.GetValue<int>("productId");
			var repoProduct = await ProductRepository.GetProduct(id);
			Product = repoProduct;
		}

		private async Task AddToCartCommandExecute()
		{
			var cartItem = await CartRepository.UpdateProduct(new CartProduct()
			{
				Id = Product.Id,
				Quantity = Quantity
			});
			if (cartItem == null)
			{
				await PageDialogService.DisplayAlertAsync("Cart Error", "We were unable to add the item to your cart", "Ok");
			}
		}
	}


}
