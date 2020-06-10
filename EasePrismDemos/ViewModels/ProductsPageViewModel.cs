using EasePrismDemos.Models;
using EasePrismDemos.Repositories;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EasePrismDemos.ViewModels
{
	public class ProductsPageViewModel : ViewModelBase, INavigatedAware
	{
		private IProductRepository ProductRepository { get; }

		private ObservableCollection<ProductSummary> _products;
		public ObservableCollection<ProductSummary> Products
		{
			get => _products;
			set => SetProperty(ref _products, value);
		}

		public DelegateCommand<ProductSummary> ViewProductDetailsCommand => 
			new DelegateCommand<ProductSummary>(async p => await NavigationService.NavigateAsync("ProductDetailPage", new NavigationParameters() { { "productId", p.Id } } ));

		public ProductsPageViewModel(
			IProductRepository productRepository, 
			INavigationService navigationService) 
			: base(navigationService)
		{
			ProductRepository = productRepository;
		}

		public override async Task InitializeAsync(INavigationParameters parameters)
		{			
			if (parameters.GetNavigationMode() == NavigationMode.Back) return;

			var repoProducts = await ProductRepository.GetProducts();
			Products = new ObservableCollection<ProductSummary>(repoProducts);			
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			InitializeAsync(parameters).Wait();
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
			
		}
	}
}
