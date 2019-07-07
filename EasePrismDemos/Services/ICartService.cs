using System.Threading.Tasks;
using EasePrismDemos.Dtos;

namespace EasePrismDemos.Services
{
	public interface ICartService
	{
		Task<CartProductDto[]> ClearAllProducts();
		Task<CartProductDto[]> GetProducts();
		Task<CartProductDto> UpdateProduct(CartProductDto product);
	}
}