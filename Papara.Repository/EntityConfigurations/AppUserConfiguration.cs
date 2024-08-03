﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
			builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);



			builder.Property(u => u.DigitalWalletBalance)
				.IsRequired()
				.HasColumnType("decimal(18,2)")
				.HasDefaultValue(0);  // Varsayılan değeri 0 olarak ayarla

			builder.Property(u => u.PointsBalance)
				.IsRequired()
				.HasColumnType("decimal(18,2)")
				.HasDefaultValue(0);  // Varsayılan değeri 0 olarak ayarla


			builder.HasOne(u => u.Basket)
				.WithOne(b => b.User)
				.HasForeignKey<Basket>(b => b.UserId)
				.OnDelete(DeleteBehavior.Restrict);  

			builder.HasOne(u => u.DigitalWallet)
				.WithOne(dw => dw.User)
				.HasForeignKey<DigitalWallet>(dw => dw.UserId)
				.OnDelete(DeleteBehavior.Restrict);  

			builder.HasMany(u => u.Orders)
				.WithOne(o => o.User)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Restrict);  
		}
	}

}
