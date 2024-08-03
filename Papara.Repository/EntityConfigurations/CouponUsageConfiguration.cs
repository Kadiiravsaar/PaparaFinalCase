using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
	{
		public void Configure(EntityTypeBuilder<CouponUsage> builder)
		{
			builder.Property(cu => cu.IsActive).HasDefaultValue(true);
			builder.Property(cu => cu.CreatedDate).IsRequired();
			builder.Property(cu => cu.UpdatedDate).IsRequired(false);
			builder.Property(cu => cu.DeletedDate).IsRequired(false);


			builder.Property(cu => cu.CouponId).IsRequired();
			builder.Property(cu => cu.UserId).IsRequired();
			builder.Property(cu => cu.UsedDate).IsRequired();

			builder.HasOne(cu => cu.Coupon)
				.WithMany(c => c.Usages)
				.HasForeignKey(cu => cu.CouponId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(cu => cu.User)
				.WithMany()
				.HasForeignKey(cu => cu.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
