using AutoMapper;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;


namespace Papara.Service.Mapping
{
	public class MapProfile : Profile
	{
		public MapProfile()
		{

			CreateMap<AppUser, AppUserResponseDTO>()
		   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
		   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
		   .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
		   .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
		   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

			CreateMap<AppRole, RoleResponseDto>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

			CreateMap<RoleRequestDto, AppRole>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


			#region Basket



			CreateMap<Basket, BasketResponseDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Select(i => new BasketItemResponseDTO
				{
					ProductId = i.ProductId,
					Quantity = i.Quantity,
					ProductName = i.Product.Name,
					Price = i.Product.Price
				})));

			// Basket to BasketWithDetailResponseDTO mappings
			CreateMap<Basket, BasketWithDetailResponseDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice))
				.ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount)) // DiscountAmount hesaplanmış olacak
				.ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.FinalPrice))         // FinalPrice hesaplanmış olacak
				.ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
				.ForMember(dest => dest.PointsEarned, opt => opt.MapFrom(src => src.PointsEarned));


			CreateMap<BasketRequestDTO, Basket>();
			CreateMap<Basket, BasketResponseDTO>();


			#endregion


			#region Test
			CreateMap<BasketItem, OrderDetail>()
		   .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
		   .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
			//.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

			CreateMap<Basket, Order>()
		  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
		  .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())  // Bu değer hesaplanmalı
		  .ForMember(dest => dest.PointUsed, opt => opt.Ignore())   // Bu değer hesaplanmalı
		  .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.Items));
			#endregion



			#region BasketItem


			CreateMap<BasketItem, BasketItemResponseDTO>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
				.ForMember(dest => dest.PointsEarned, opt => opt.MapFrom(src => src.PointsEarned))
				.ForMember(dest => dest.PointsPercentage, opt => opt.MapFrom(src => src.Product.PointsPercentage))
				.ForMember(dest => dest.MaxPoint, opt => opt.MapFrom(src => src.Product.MaxPoint));

			
			CreateMap<BasketItem, BasketItemWithDetailResponseDTO>()
				.ForMember(dest => dest.BasketItemId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Basket.UserId));


			CreateMap<BasketItemRequestDTO, BasketItem>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.BasketId, opt => opt.MapFrom(src => src.BasketId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

			#endregion




			#region Category

			CreateMap<Category, CategoryResponseDTO>();

			CreateMap<Category, CategoryWithDetailResponseDTO>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)));

			CreateMap<CategoryRequestDTO, Category>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
			.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));


			#endregion


			#region Coupon


			CreateMap<CouponRequestDTO, Coupon>()
				.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
				.ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
				.ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
				.ReverseMap();

			CreateMap<Coupon, CouponResponseDTO>()
				.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
				.ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
				.ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
				.ReverseMap();

			CreateMap<Coupon, CouponWithDetailResponseDTO>()
				.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
				.ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
				.ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
				.ForMember(dest => dest.Baskets, opt => opt.MapFrom(src => src.Baskets))
				.ForMember(dest => dest.Usages, opt => opt.MapFrom(src => src.Usages));


			#endregion











			#region CouponUsage

			CreateMap<CouponUsage, CouponUsageResponseDTO>();


			CreateMap<CouponUsage, CouponUsageResponseDTO>().ReverseMap();
			
			CreateMap<CouponUsage, CouponUsageWithDetailResponseDTO>()
				.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Coupon.CouponCode))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => src.Coupon))
				.ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
				.ForMember(dest => dest.Basket, opt => opt.MapFrom(src => src.Basket));


			CreateMap<CouponUsageRequestDTO, CouponUsage>()
			.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

			CreateMap<CouponUsageRequestDTO, CouponUsage>().ReverseMap();



			#endregion


			#region DigitalWallet

			CreateMap<DigitalWallet, DigitalWalletWithDetailResponseDTO>()
			   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			   .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
			   .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
			   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty));


			CreateMap<DigitalWalletRequestDTO, DigitalWallet>() // sadece admin buraya müdahale etmeli kullanıcı bunu payment üzerinden yönetmeli controlelrda açıklama yapldı
			.ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));
			


			CreateMap<DigitalWallet, DigitalWalletResponseDTO>()
			  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			  .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
			  .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points));

			#endregion




			#region Order

			CreateMap<Order, OrderResponseDTO>()
			   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			   .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
			   .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
			   .ForMember(dest => dest.PointUsed, opt => opt.MapFrom(src => src.PointUsed))
			   .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

			CreateMap<Order, OrderWithDetailResponseDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)) 
				.ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
				.ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
				.ForMember(dest => dest.PointUsed, opt => opt.MapFrom(src => src.PointUsed))
				.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

			CreateMap<OrderRequestDTO, Order>()
				.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
				.ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
				.ForMember(dest => dest.PointUsed, opt => opt.MapFrom(src => src.PointUsed))
				.ReverseMap();

			#endregion


			#region OrderDetail

			CreateMap<OrderDetail, OrderDetailResponseDTO>()
			   .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
			   .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
			   .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));


			CreateMap<OrderDetailRequestDTO, OrderDetail>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));


			CreateMap<OrderDetail, OrderDetailWithDetailResponseDTO>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
				.ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
				.ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));
				

			#endregion




			#region Product

			CreateMap<ProductRequestDTO, Product>()
			  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			  .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			  .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
			  .ForMember(dest => dest.PointsPercentage, opt => opt.MapFrom(src => src.PointsPercentage))
			  .ForMember(dest => dest.MaxPoint, opt => opt.MapFrom(src => src.MaxPoint))
			  .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
			  .ForMember(dest => dest.ProductCategories, opt => opt.MapFrom((src, dest) =>
				  src.CategoryIds != null ? src.CategoryIds.Select(id => new ProductCategory { CategoryId = id, ProductId = dest.Id }).ToList() : new List<ProductCategory>()));


			CreateMap<ProductResponseDTO, ProductRequestDTO>()
			 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			 .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
			 .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
			 .ForMember(dest => dest.PointsPercentage, opt => opt.MapFrom(src => src.PointsPercentage))
			 .ForMember(dest => dest.MaxPoint, opt => opt.MapFrom(src => src.MaxPoint));


			CreateMap<Product, ProductResponseDTO>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))

				.ForMember(dest => dest.PointsPercentage, opt => opt.MapFrom(src => src.PointsPercentage))
				.ForMember(dest => dest.MaxPoint, opt => opt.MapFrom(src => src.MaxPoint))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

			CreateMap<Product, ProductWithDetailResponseDTO>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
				.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
				.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
				.ForMember(dest => dest.PointsPercentage, opt => opt.MapFrom(src => src.PointsPercentage))
				.ForMember(dest => dest.MaxPoints, opt => opt.MapFrom(src => src.MaxPoint))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
				.ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => new CategoryResponseDTO
				{
					Id = pc.Category.Id,
					Name = pc.Category.Name,
					Url = pc.Category.Url,
					Tags = pc.Category.Tags
				}).ToList()));

			
			CreateMap<Category, CategoryWithDetailResponseDTO>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)))
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => new ProductResponseDTO
				{
					Id = pc.Product.Id,
					Name = pc.Product.Name,
					Description = pc.Product.Description,
					Price = pc.Product.Price,
					PointsPercentage = pc.Product.PointsPercentage,
					MaxPoint = pc.Product.MaxPoint,
					Stock = pc.Product.Stock
				}).ToList()));
			


			CreateMap<ProductRequestDTO, Product>()
			.ForMember(dest => dest.ProductCategories, opt => opt.MapFrom((src, dest) =>
				src.CategoryIds.Select(id => new ProductCategory { CategoryId = id, ProductId = dest.Id }).ToList()));

			CreateMap<ProductRequestDTO, Product>()
			.ForMember(dest => dest.ProductCategories, opt => opt.MapFrom(src =>
				src.CategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList()));


			#endregion


			#region ProductCategory

			CreateMap<ProductCategory, ProductCategoryWithDetailResponseDTO>()
						.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
						.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
						.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
						.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
						.ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
						.ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
						.ForMember(dest => dest.CategoryUrl, opt => opt.MapFrom(src => src.Category.Url))
						.ForMember(dest => dest.CategoryTags, opt => opt.MapFrom(src => src.Category.Tags));


			CreateMap<ProductCategoryRequestDTO, ProductCategory>()
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

			CreateMap<ProductCategory, ProductCategoryResponseDTO>() // dene ve sil
				.ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
				.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

			CreateMap<ProductCategory, ProductCategoryResponseDTO>().ReverseMap();

			#endregion


		}
	}
}
