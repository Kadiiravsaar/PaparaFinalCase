using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using Papara.Repository.EntityConfigurations;
using Papara.Repository.Repositories;
using System.Collections.Generic;


namespace Papara.Repository.Context
{
	public class MsSqlDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options)
			: base(options)
		{
		}


		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Coupon> Coupons { get; set; }
		public DbSet<CouponUsage> CouponUsages { get; set; }
		public DbSet<DigitalWallet> DigitalWallets { get; set; }
		public DbSet<Stock> Stocks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new AppUserConfiguration());
			modelBuilder.ApplyConfiguration(new BasketConfiguration());
			modelBuilder.ApplyConfiguration(new BasketItemConfiguration());
			modelBuilder.ApplyConfiguration(new ProductConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
			modelBuilder.ApplyConfiguration(new OrderConfiguration());
			modelBuilder.ApplyConfiguration(new CouponConfiguration());
			modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
			modelBuilder.ApplyConfiguration(new CouponUsageConfiguration());
			modelBuilder.ApplyConfiguration(new DigitalWalletConfiguration());
			modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new StockConfiguration());

			#region Seed-Data
			modelBuilder.Entity<AppRole>().HasData(
			new AppRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
			new AppRole { Id = "2", Name = "User", NormalizedName = "USER" });

			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
			new IdentityUserRole<string> { RoleId = "1", UserId = "4873b151-f87b-43f1-b1f4-892864c21c45" }, // Admin kullanıcısına Admin rolünü atar
			new IdentityUserRole<string> { RoleId = "2", UserId = "5c27148d-4a49-491c-8ef2-ec0ad4e8f13f" }  // User kullanıcısına User rolünü atar
		);
			var hasher = new PasswordHasher<AppUser>();
			modelBuilder.Entity<AppUser>().HasData(
				new AppUser
				{
					Id = "4873b151-f87b-43f1-b1f4-892864c21c45",
					UserName = "admin",
					NormalizedUserName = "ADMIN",
					Email = "a@gmail.com",
					NormalizedEmail = "A@GMAIL.COM",
					EmailConfirmed = true,
					PasswordHash = hasher.HashPassword(null, "asd123"),
					SecurityStamp = string.Empty,
					FirstName = "Admin",
					LastName = "Admin",
					CreatedDate = DateTime.UtcNow

				},
				new AppUser
				{
					Id = "5c27148d-4a49-491c-8ef2-ec0ad4e8f13f",
					UserName = "user",
					NormalizedUserName = "USER",
					Email = "u@gmail.com",
					NormalizedEmail = "U@GMAIL.COM",
					EmailConfirmed = true,
					PasswordHash = hasher.HashPassword(null, "asd123"),
					SecurityStamp = string.Empty,
					FirstName = "User",
					LastName = "User",
					CreatedDate = DateTime.UtcNow

				});

			modelBuilder.Entity<Product>().HasData(
			new Product
			{
				Id = 1,
				Name = "Bluetooth Kulaklık",
				Description = "Yüksek kaliteli ses deneyimi sunan kablosuz kulaklık.",
				Price = 300.00m,
				PointsPercentage = 2.5m,
				MaxPoint = 15m
			},
			new Product
			{
				Id = 2,
				Name = "Akıllı Saat",
				Description = "Sağlık ve fitness takibi yapabilen akıllı saat.",
				Price = 500.00m,
				PointsPercentage = 3.0m,
				MaxPoint = 20m
			},
			new Product
			{
				Id = 3,
				Name = "E-Kitap Okuyucu",
				Description = "Göz yormayan ekranı ile uzun süreli okuma için e-kitap okuyucu.",
				Price = 250m,
				PointsPercentage = 1.0m,
				MaxPoint = 10m
			},
			new Product
			{
				Id = 4,
				Name = "Kablosuz Şarj Cihazı",
				Description = "Çeşitli cihazlar için uyumlu hızlı kablosuz şarj cihazı.",
				Price = 150m,
				PointsPercentage = 1.5m,
				MaxPoint = 5m
			},
			new Product
			{
				Id = 5,
				Name = "Akıllı Lamba",
				Description = "Uzaktan kontrol edilebilen ve renk değiştirebilen LED akıllı lamba.",
				Price = 90m,
				PointsPercentage = 2.0m,
				MaxPoint = 7m
			}
		);
			#endregion

		}
	}
}
