using AutoMapper;
using EasePrismDemos.Dtos;
using EasePrismDemos.Models;
using DryIoc;

namespace EasePrismDemos
{
	public static class AutoMapperConfig
	{
		public static void RegisterMapper(Container container)
		{
			container.UseInstance(ConfigureMapper());
		}

		public static IMapper ConfigureMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CartProductDto, CartProduct>().ReverseMap();
				cfg.CreateMap<OrderDto, Order>().ReverseMap();
				cfg.CreateMap<OrderProductRequestDto, OrderProductRequest>().ReverseMap();
				cfg.CreateMap<OrderProductSummaryDto, OrderProductSummary>().ReverseMap();
				cfg.CreateMap<OrderSummaryDto, OrderSummary>().ReverseMap();
				cfg.CreateMap<ProductDto, Product>().ReverseMap();
				cfg.CreateMap<ProductSummaryDto, ProductSummary>().ReverseMap();
			});
			var mapper = config.CreateMapper();
			return mapper;
		}
	}
}
