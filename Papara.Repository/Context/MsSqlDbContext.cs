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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // olmasa ne olur ne işe yarar

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


		}


	}




}
