using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(o => o.IsActive).HasDefaultValue(true);
			builder.Property(o => o.CreatedDate).IsRequired(true);
			builder.Property(o => o.UpdatedDate).IsRequired(false);
			builder.Property(o => o.DeletedDate).IsRequired(false);

			builder.Property(o => o.UserId).IsRequired();
			builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(9);

			builder.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(o => o.CouponAmount).IsRequired(false).HasColumnType("decimal(18,2)");
			builder.Property(o => o.PointsUsed).IsRequired(false).HasColumnType("decimal(18,2)");


			builder.HasOne(o => o.User)
				.WithMany()
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.Coupon)
			.WithMany(c => c.Orders)
			.HasForeignKey(o => o.CouponId)
			.OnDelete(DeleteBehavior.Restrict);


			builder.HasMany(o => o.OrderDetails)
				.WithOne(od => od.Order)
				.HasForeignKey(od => od.OrderId)
				.OnDelete(DeleteBehavior.Restrict); 
		}
	}

}
