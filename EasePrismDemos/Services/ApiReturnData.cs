using System.Net;

namespace EasePrismDemos.Services
{
	public class ApiReturnData<T>
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorMessage { get; set; }
		public T Data { get; set; }
	}
}
