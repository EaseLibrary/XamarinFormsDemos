using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace EasePrismDemos.ViewModels
{
	public abstract class ViewModelBase : BindableBase, IInitializeAsync
	{
		protected INavigationService NavigationService { get; }

		public ViewModelBase(INavigationService navigationService)
		{
			NavigationService = navigationService;
		}
				
		public virtual Task InitializeAsync(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}
	}


}
