using System.Net.Http;

namespace EasePrismDemos.Services
{
	public interface IHttpMessageHandlerFactory
	{
		HttpMessageHandler GetHttpMessageHandler();
	}
}
