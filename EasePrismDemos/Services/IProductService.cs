using System.Threading.Tasks;
using EasePrismDemos.Dtos;

namespace EasePrismDemos.Services
{
	public interface IProductService
	{
		Task<ProductDto> GetProduct(int productId);
		Task<ProductSummaryDto[]> GetProducts();
	}
}