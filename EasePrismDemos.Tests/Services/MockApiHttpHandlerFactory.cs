using EasePrismDemos.Dtos;
using EasePrismDemos.Services;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasePrismDemos.Tests.Services
{
	public class MockApiHttpHandlerFactory : IHttpMessageHandlerFactory
	{
		public MockHttpMessageHandler MessageHandler { get; set; }

		private TestApiData Data { get; }

		public MockApiHttpHandlerFactory()
		{
			Data = new TestApiData();
			MessageHandler = new MockHttpMessageHandler();
			SetupProductsApi();
			SetupOrdersApi();
			SetupCartApi();
		}

		private void SetupProductsApi()
		{
			MessageHandler.When(HttpMethod.Get,
				ApiEndpoints.GetProductsUri().ToString())
				.Respond("application/json", JsonConvert.SerializeObject(Data.Products.Cast<ProductSummaryDto>()));

			MessageHandler.When(HttpMethod.Get,
				$"{ApiEndpoints.GetProductsUri()}/*")
				.Respond(request =>
				{
					int productId = GetIdFromUrl(request);

					var product = Data.Products.FirstOrDefault(p => p.Id == productId);

					return CreateJsonResponse(product);
				});

		}

		private void SetupOrdersApi()
		{
			MessageHandler.When(HttpMethod.Get,
				ApiEndpoints.GetOrdersUri().ToString())
				.Respond("application/json", JsonConvert.SerializeObject(Data.Orders.Cast<OrderSummaryDto>()));

			MessageHandler.When(HttpMethod.Get,
				$"{ApiEndpoints.GetOrdersUri()}/*")
				.Respond(request =>
				{
					var orderId = GetIdFromUrl(request);

					var order = Data.Orders.FirstOrDefault(o => o.Id == orderId);

					return CreateJsonResponse(order);
				});

			MessageHandler.When(HttpMethod.Post,
				$"{ApiEndpoints.GetOrdersUri()}")
				.Respond(async request =>
				{
					var orderRequest = await GetObjectFromRequest<OrderProductRequestDto[]>(request);

					var nextOrderId = Data.Orders.Max(o => o.Id) + 1;
					var order = Data.CreateOrderDto(nextOrderId, DateTime.Now,
						orderRequest.Select(o => Data.CreateOrderProductSummaryDto(o.Id, o.Quantity)).ToArray());

					Data.Orders.Add(order);

					return CreateJsonResponse(order);

				});

		}

		private void SetupCartApi()
		{
			MessageHandler.When(HttpMethod.Get,
				ApiEndpoints.GetCartUri().ToString())
				.Respond("application/json", JsonConvert.SerializeObject(Data.Cart));

			MessageHandler.When(HttpMethod.Delete,
				ApiEndpoints.DeleteCartUri().ToString())
				.Respond(request =>
				{
					Data.Cart.Clear();
					return CreateJsonResponse(Data.Cart);
				});

			MessageHandler.When(HttpMethod.Post,
				$"{ApiEndpoints.PostCartProductUri()}")
				.Respond(async request =>
				{
					var cartRequest = await GetObjectFromRequest<CartProductDto>(request);

					var currentItem = Data.Cart.FirstOrDefault(c => c.Id == cartRequest.Id);
					if (currentItem != null)
					{
						if (cartRequest.Quantity == 0)
						{
							Data.Cart.Remove(currentItem);
						}
						else
						{
							currentItem.Quantity = cartRequest.Quantity;
						}
					}
					else if (cartRequest.Quantity > 0)
					{
						Data.Cart.Add(cartRequest);
					}

					return CreateJsonResponse(cartRequest);
				});

		}

		private static int GetIdFromUrl(HttpRequestMessage request)
		{
			var idSegment = request.RequestUri.Segments.Length - 1;
			var id = int.Parse(request.RequestUri.Segments[idSegment]);
			return id;
		}

		private static async Task<T> GetObjectFromRequest<T>(HttpRequestMessage request)
		{
			var requestJson = await request.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(requestJson);
		}

		private HttpResponseMessage CreateJsonResponse(object data)
		{
			if (data == null)
			{
				return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
			}

			var json = JsonConvert.SerializeObject(data);
			return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
			};
		}

		public HttpMessageHandler GetHttpMessageHandler()
		{
			return MessageHandler;
		}
	}
}
