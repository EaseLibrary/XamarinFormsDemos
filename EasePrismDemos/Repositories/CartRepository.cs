using EasePrismDemos.Dtos;
using EasePrismDemos.Models;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public class CartRepository : ICartRepository
	{
		public AutoMapper.IMapper Mapper { get; }
		public Services.ICartService CartService { get; }

		public CartRepository(AutoMapper.IMapper mapper,
			Services.ICartService cartService)
		{
			Mapper = mapper;
			CartService = cartService;
		}

		public async Task<CartProduct> UpdateProduct(CartProduct product)
		{
			var requestDto = Mapper.Map<CartProduct, CartProductDto>(product);
			var responseDto = await CartService.UpdateProduct(requestDto);
			return Mapper.Map<CartProductDto, CartProduct>(responseDto);
		}

		public async Task<CartProduct[]> GetProducts()
		{
			var products = await CartService.GetProducts();
			return Mapper.Map<CartProductDto[], CartProduct[]>(products);
		}

		public async Task<CartProduct[]> ClearAllProducts()
		{
			var products = await CartService.ClearAllProducts();
			return Mapper.Map<CartProductDto[], CartProduct[]>(products);
		}
	}
}
