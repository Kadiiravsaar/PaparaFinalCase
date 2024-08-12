using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
		}

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
					Email = "admin@gmail.com",
					NormalizedEmail = "ADMIN@GMAIL.COM",
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
					Email = "user@gmail.com",
					NormalizedEmail = "USER@GMAIL.COM",
					EmailConfirmed = true,
					PasswordHash = hasher.HashPassword(null, "asd123"),
					SecurityStamp = string.Empty,
					FirstName = "User",
					LastName = "User",
					CreatedDate = DateTime.UtcNow

				});

			
			#endregion

		}
	}
}
