using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
	{
		public void Configure(EntityTypeBuilder<OrderDetail> builder)
		{
			builder.Property(od => od.IsActive).HasDefaultValue(true);
			builder.Property(od => od.CreatedDate).IsRequired();
			builder.Property(od => od.UpdatedDate).IsRequired(false);
			builder.Property(od => od.DeletedDate).IsRequired(false);

			builder.Property(od => od.OrderId).IsRequired();
			builder.Property(od => od.ProductId).IsRequired();
			builder.Property(od => od.Quantity).IsRequired();
			builder.Property(od => od.Price).IsRequired().HasColumnType("decimal(18,2)");

			builder.HasOne(od => od.Order)
				.WithMany(o => o.OrderDetails)
				.HasForeignKey(od => od.OrderId)
				.OnDelete(DeleteBehavior.Restrict);  

			builder.HasOne(od => od.Product)
				.WithMany()
				.HasForeignKey(od => od.ProductId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
