using EasePrismDemos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasePrismDemos.Tests
{
	public class TestApiData
	{
		public List<ProductDto> Products { get; set; }
		public List<OrderDto> Orders { get; set; }
		public List<CartProductDto> Cart { get; set; }

		public TestApiData()
		{
			SetupData();
		}

		private void SetupData()
		{
			Products = new List<ProductDto>
			{
				CreateProductDto(1, "First Product", 10.00m),
				CreateProductDto(2, "Second Product", 9.00m),
				CreateProductDto(3, "Third Product", 8.00m),
			};

			Orders = new List<OrderDto>
			{
				CreateOrderDto(1, DateTime.Now.AddDays(-10),
					new[]
					{
						CreateOrderProductSummaryDto(1, 3, null),
						CreateOrderProductSummaryDto(2, 2, null),
						CreateOrderProductSummaryDto(3, 1, 5m),
					})
			};

			Cart = new List<CartProductDto>
			{
				CreateCartProductDto(2, 3),
			};
		}

		public ProductDto CreateProductDto(int id, string name, decimal price)
		{
			return new ProductDto
			{
				Id = id,
				Name = "Third Product",
				Price = 8.00m,
				Description = $"Item {id} Description",
				ImageUrls = new[] { "http://server/images/product{id}" }
			};
		}

		public OrderDto CreateOrderDto(int id, DateTime orderDate, OrderProductSummaryDto[] products)
		{
			return new OrderDto
			{
				Id = id,
				OrderPlaced = orderDate,
				Products = products,
				Total = products.Sum(p => p.Price * p.Quantity)
			};
		}

		public OrderProductSummaryDto CreateOrderProductSummaryDto(int productId, int quantity, decimal? priceOverride = null)
		{
			var product = Products.First(p => p.Id == productId);
			return new OrderProductSummaryDto
			{
				Id = product.Id,
				Name = product.Name,
				Quantity = quantity,
				Price = priceOverride ?? product.Price,
			};
		}

		public CartProductDto CreateCartProductDto(int productId, int quantity)
		{
			var product = Products.First(p => p.Id == productId);
			return new CartProductDto
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Quantity = quantity,
			};
		}
	}
}
