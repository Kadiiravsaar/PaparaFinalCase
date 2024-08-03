using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
	{
		public void Configure(EntityTypeBuilder<BasketItem> builder)
		{
			builder.Property(bi => bi.IsActive).HasDefaultValue(true);
			builder.Property(bi => bi.CreatedDate).IsRequired();
			builder.Property(bi => bi.UpdatedDate).IsRequired(false);
			builder.Property(bi => bi.DeletedDate).IsRequired(false);


			builder.Property(bi => bi.ProductId).IsRequired();
			builder.Property(bi => bi.BasketId).IsRequired();
			builder.Property(bi => bi.Quantity).IsRequired();

			builder.HasOne(bi => bi.Product)
				.WithMany()
				.HasForeignKey(bi => bi.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(bi => bi.Basket)
				.WithMany(b => b.Items)
				.HasForeignKey(bi => bi.BasketId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
