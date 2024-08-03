using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.Property(c => c.IsActive).HasDefaultValue(true);
			builder.Property(c => c.CreatedDate).IsRequired();
			builder.Property(c => c.UpdatedDate).IsRequired(false);
			builder.Property(c => c.DeletedDate).IsRequired(false);

			builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

			builder.Property(c => c.Url).IsRequired().HasMaxLength(200);

			builder.Property(c => c.Tags).HasMaxLength(500);

			builder.HasMany(c => c.ProductCategories)
				.WithOne(pc => pc.Category)
				.HasForeignKey(pc => pc.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);  
		}
	}

}
