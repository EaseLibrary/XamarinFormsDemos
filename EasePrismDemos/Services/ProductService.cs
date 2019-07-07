using EasePrismDemos.Dtos;
using Prism.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasePrismDemos.Services
{
	public class ProductService : ApiServiceBase, IProductService
	{
		public ProductService(IHttpMessageHandlerFactory messageHandlerFactory)
			: base(messageHandlerFactory)
		{
		}

		public async Task<ProductDto> GetProduct(int productId)
		{
			var result = await ApiGetData<ProductDto>(ApiEndpoints.GetProductUri(productId));
			return result.Data;
		}

		public async Task<ProductSummaryDto[]> GetProducts()
		{
			var result = await ApiGetData<ProductSummaryDto[]>(ApiEndpoints.GetProductsUri());
			return result.Data;
		}
	}
}
