using EasePrismDemos.Models;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public interface ICartRepository
	{
		Task<CartProduct[]> ClearAllProducts();
		Task<CartProduct[]> GetProducts();
		Task<CartProduct> UpdateProduct(CartProduct product);
	}
}