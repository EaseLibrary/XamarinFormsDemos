using System;

namespace EasePrismDemos.Services
{
	public static class ApiEndpoints
	{
		private static string Protocol = "http";
		private static string ServerName = "EasePrismDemos.com";

		private static Uri _baseUri;
		private static Uri BaseUri => _baseUri ?? (_baseUri = new Uri($"{Protocol}://{ServerName}/api/"));

		public static Uri GetProductsUri() => new Uri(BaseUri, $"products");
		public static Uri GetProductUri(int productId) => new Uri(BaseUri, $"products/{productId}");

		public static Uri PostOrderUri() => new Uri(BaseUri, $"orders");
		public static Uri GetOrdersUri() => new Uri(BaseUri, $"orders");
		public static Uri GetOrderUri(int orderId) => new Uri(BaseUri, $"orders/{orderId}");

		public static Uri GetCartUri() => new Uri(BaseUri, $"cart");
		public static Uri DeleteCartUri() => new Uri(BaseUri, $"cart");
		public static Uri PostCartProductUri() => new Uri(BaseUri, $"cart/product");
	}
}
