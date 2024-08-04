using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Repository.EntityConfigurations
{
	public class BasketConfiguration : IEntityTypeConfiguration<Basket>
	{
		public void Configure(EntityTypeBuilder<Basket> builder)
		{
			builder.Property(b => b.IsActive).HasDefaultValue(true);
			builder.Property(b => b.CreatedDate).IsRequired();
			builder.Property(b => b.UpdatedDate).IsRequired(false);
			builder.Property(b => b.DeletedDate).IsRequired(false);

			builder.Property(b => b.UserId).IsRequired();
			builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");


			builder.HasOne(b => b.Coupon)
				   .WithMany(c => c.Baskets)
				   .HasForeignKey(b => b.CouponId)
				   .IsRequired(false)  // Kupon ID isteğe bağlıdır
				   .OnDelete(DeleteBehavior.SetNull);  // Kupon silindiğinde sepet üzerindeki referans null'a ayarlanmalı


			builder.HasOne(b => b.User)
					   .WithOne(u => u.Basket)
					   .HasForeignKey<Basket>(b => b.UserId)
					   .OnDelete(DeleteBehavior.Restrict); 

			builder.HasMany(b => b.Items)
				.WithOne(b => b.Basket)
				.HasForeignKey(b => b.BasketId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict); 

		}
	}

}
