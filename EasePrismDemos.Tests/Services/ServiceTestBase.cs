using Ease.NUnit.DryIoc.PrismForms;
using EasePrismDemos.Services;

namespace EasePrismDemos.Tests.Services
{
	public class ServiceTestBase : AppTestBase
	{
		public ServiceTestBase()
		{
			RegisterType<IHttpMessageHandlerFactory, MockApiHttpHandlerFactory>();
		}
	}
}
