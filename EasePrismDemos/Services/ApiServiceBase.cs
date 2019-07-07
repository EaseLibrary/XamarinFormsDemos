using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasePrismDemos.Services
{
	public class ApiServiceBase
	{
		protected TimeSpan TimeoutSpan = TimeSpan.FromSeconds(15);
		protected TimeSpan FileUploadTimeoutSpan = TimeSpan.FromMinutes(2);
		protected IHttpMessageHandlerFactory MessageHandlerFactory { get; }

		protected ApiServiceBase(IHttpMessageHandlerFactory messageHandlerFactory)
		{
			MessageHandlerFactory = messageHandlerFactory;
		}

		protected async Task<ApiReturnData<TReturnType>> ApiGetData<TReturnType>(Uri apiUri)
		{
			try
			{
				using (var client = new HttpClient(MessageHandlerFactory.GetHttpMessageHandler()) { Timeout = TimeoutSpan })
				{
					HttpResponseMessage apiResult = await client.GetAsync(apiUri);

					string json = null;

					json = apiResult.Content.ReadAsStringAsync().Result;

					if (!apiResult.IsSuccessStatusCode)
					{
						return new ApiReturnData<TReturnType>()
						{
							StatusCode = apiResult.StatusCode,
							ErrorMessage = apiResult.ReasonPhrase
						};
					}
					var result = JsonConvert.DeserializeObject<TReturnType>(json);
					return new ApiReturnData<TReturnType>()
					{
						Data = result,
						StatusCode = apiResult.StatusCode
					};
				}
			}
			catch (Exception ex)
			{
				return new ApiReturnData<TReturnType>
				{
					Data = default(TReturnType),
					ErrorMessage = ex.Message,
					StatusCode = HttpStatusCode.BadRequest
				};
			}
		}

		protected async Task<ApiReturnData<TReturnType>> ApiPostData<TReturnType>(Uri uri, object data)
		{
			try
			{
				using (var client = new HttpClient(MessageHandlerFactory.GetHttpMessageHandler()) { Timeout = TimeoutSpan })
				{

					var jsonData = JsonConvert.SerializeObject(data, Formatting.None,
						new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified });
					var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

					var apiResult = await client.PostAsync(uri, httpContent);

					var json = apiResult.Content.ReadAsStringAsync().Result;

					if (!apiResult.IsSuccessStatusCode)
					{
						return new ApiReturnData<TReturnType>()
						{
							StatusCode = apiResult.StatusCode,
							ErrorMessage = apiResult.ReasonPhrase
						};
					}

					var result = JsonConvert.DeserializeObject<TReturnType>(json);
					return new ApiReturnData<TReturnType>()
					{
						Data = result,
						StatusCode = apiResult.StatusCode
					};

				}
			}
			catch (Exception ex)
			{
				return new ApiReturnData<TReturnType>
				{
					Data = default(TReturnType),
					ErrorMessage = ex.Message,
					StatusCode = HttpStatusCode.BadRequest
				};
			}
		}

		protected async Task<ApiReturnData<TReturnType>> ApiPutData<TReturnType>(Uri uri, object data)
		{
			try
			{
				using (var client = new HttpClient(MessageHandlerFactory.GetHttpMessageHandler()) { Timeout = TimeoutSpan })
				{
					var jsonData = JsonConvert.SerializeObject(data, Formatting.None,
						new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified });
					var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

					var apiResult = await client.PutAsync(uri, httpContent);

					var json = apiResult.Content.ReadAsStringAsync().Result;

					if (!apiResult.IsSuccessStatusCode)
					{
						return new ApiReturnData<TReturnType>()
						{
							StatusCode = apiResult.StatusCode,
							ErrorMessage = apiResult.ReasonPhrase
						};
					}

					var result = JsonConvert.DeserializeObject<TReturnType>(json);
					return new ApiReturnData<TReturnType>()
					{
						Data = result,
						StatusCode = apiResult.StatusCode
					};
				}
			}
			catch (Exception ex)
			{
				return new ApiReturnData<TReturnType>
				{
					Data = default(TReturnType),
					ErrorMessage = ex.Message,
					StatusCode = HttpStatusCode.BadRequest
				};
			}
		}

		protected async Task<ApiReturnData<TReturnType>> ApiDelete<TReturnType>(Uri uri)
		{
			try
			{
				using (var client = new HttpClient(MessageHandlerFactory.GetHttpMessageHandler()) { Timeout = TimeoutSpan })
				{
					var apiResult = await client.DeleteAsync(uri);

					var json = apiResult.Content.ReadAsStringAsync().Result;

					if (!apiResult.IsSuccessStatusCode)
					{
						return new ApiReturnData<TReturnType>()
						{
							StatusCode = apiResult.StatusCode,
							ErrorMessage = apiResult.ReasonPhrase
						};
					}

					var result = JsonConvert.DeserializeObject<TReturnType>(json);
					return new ApiReturnData<TReturnType>()
					{
						Data = result,
						StatusCode = apiResult.StatusCode
					};
				}
			}
			catch (Exception ex)
			{
				return new ApiReturnData<TReturnType>
				{
					Data = default(TReturnType),
					ErrorMessage = ex.Message,
					StatusCode = HttpStatusCode.BadRequest
				};
			}
		}

	}
}
