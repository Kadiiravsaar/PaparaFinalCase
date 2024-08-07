using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.IsActive).HasDefaultValue(true);
			builder.Property(p => p.CreatedDate).IsRequired();
			builder.Property(p => p.UpdatedDate).IsRequired(false);
			builder.Property(p => p.DeletedDate).IsRequired(false);


			builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
			builder.Property(p => p.Description).HasMaxLength(500);
			builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(p => p.PointsPercentage).IsRequired().HasColumnType("decimal(5,2)");
			builder.Property(p => p.MaxPoint).IsRequired().HasColumnType("decimal(18,2)");

			builder.HasMany(p => p.ProductCategories)
				.WithOne(pc => pc.Product)
				.HasForeignKey(pc => pc.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			
		}
	}

}
