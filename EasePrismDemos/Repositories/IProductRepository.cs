using EasePrismDemos.Models;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public interface IProductRepository
	{
		Task<Product> GetProduct(int productId);
		Task<ProductSummary[]> GetProducts();
	}
}