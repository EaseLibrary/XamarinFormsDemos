using EasePrismDemos.Dtos;
using System.Threading.Tasks;

namespace EasePrismDemos.Services
{
	public class CartService : ApiServiceBase, ICartService
	{
		public CartService(IHttpMessageHandlerFactory messageHandlerFactory)
			: base(messageHandlerFactory)
		{
		}

		public async Task<CartProductDto> UpdateProduct(CartProductDto product)
		{
			var result = await ApiPostData<CartProductDto>(ApiEndpoints.PostCartProductUri(), product);
			return result.Data;
		}

		public async Task<CartProductDto[]> GetProducts()
		{
			var result = await ApiGetData<CartProductDto[]>(ApiEndpoints.GetCartUri());
			return result.Data;
		}

		public async Task<CartProductDto[]> ClearAllProducts()
		{
			var result = await ApiDelete<CartProductDto[]>(ApiEndpoints.DeleteCartUri());
			return result.Data;
		}

	}
}
