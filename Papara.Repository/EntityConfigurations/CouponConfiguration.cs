using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
	{
		public void Configure(EntityTypeBuilder<Coupon> builder)
		{

			builder.Property(c => c.IsActive).HasDefaultValue(true);
			builder.Property(c => c.CreatedDate).IsRequired(true);
			builder.Property(c => c.UpdatedDate).IsRequired(false);
			builder.Property(c => c.DeletedDate).IsRequired(false);


			builder.Property(c => c.CouponCode).IsRequired().HasMaxLength(10);

			builder.Property(c => c.Amount).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(c => c.ExpiryDate).IsRequired();



			builder.HasMany(c => c.Orders)
				   .WithOne(o => o.Coupon)
				   .HasForeignKey(o => o.CouponId)
				   .OnDelete(DeleteBehavior.SetNull); // Kupon silindiğinde sipariş üzerindeki referans null'a ayarlanmalı

			builder.HasMany(c => c.Usages)
				   .WithOne(u => u.Coupon)
				   .HasForeignKey(u => u.CouponId)
				   .OnDelete(DeleteBehavior.Restrict); // Kupon kullanımı silindiğinde kupon silinmemeli

		}
	}

}
