using EasePrismDemos.Dtos;
using EasePrismDemos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasePrismDemos.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private AutoMapper.IMapper Mapper { get; }
		private Services.IProductService ProductService { get; }

		public ProductRepository(AutoMapper.IMapper mapper,
			Services.IProductService productService)
		{
			Mapper = mapper;
			ProductService = productService;
		}

		public async Task<Product> GetProduct(int productId)
		{
			var product = await ProductService.GetProduct(productId);
			return Mapper.Map<ProductDto, Product>(product);
		}

		public async Task<ProductSummary[]> GetProducts()
		{
			var products = await ProductService.GetProducts();
			return Mapper.Map<ProductSummaryDto[], ProductSummary[]>(products);
		}
	}
}
