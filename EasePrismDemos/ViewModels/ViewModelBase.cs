using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace EasePrismDemos.ViewModels
{
	public abstract class ViewModelBase : BindableBase, INavigatedAwareAsync
	{
		protected INavigationService NavigationService { get; }

		public ViewModelBase(INavigationService navigationService)
		{
			NavigationService = navigationService;
		}
		
		public virtual Task OnNavigatedToAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}
	}


}
