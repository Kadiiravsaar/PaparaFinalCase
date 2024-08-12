using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
	{
		public void Configure(EntityTypeBuilder<ProductCategory> builder)
		{
			builder.Property(pc => pc.IsActive).HasDefaultValue(true);
			builder.Property(pc => pc.CreatedDate).IsRequired();
			builder.Property(pc => pc.UpdatedDate).IsRequired(false);
			builder.Property(pc => pc.DeletedDate).IsRequired(false);

			
			builder.Property(pc => pc.ProductId).IsRequired();
			builder.Property(pc => pc.CategoryId).IsRequired();

		
			builder.HasOne(pc => pc.Product)
				.WithMany(p => p.ProductCategories)
				.HasForeignKey(pc => pc.ProductId)
				.OnDelete(DeleteBehavior.Cascade); 

			
			builder.HasOne(pc => pc.Category)
				.WithMany(c => c.ProductCategories)
				.HasForeignKey(pc => pc.CategoryId)
				.OnDelete(DeleteBehavior.Restrict); 
		}
	}

}
