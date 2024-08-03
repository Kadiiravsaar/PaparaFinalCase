using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;

namespace Papara.Repository.EntityConfigurations
{
	public class DigitalWalletConfiguration : IEntityTypeConfiguration<DigitalWallet>
	{
		public void Configure(EntityTypeBuilder<DigitalWallet> builder)
		{
			builder.Property(dw => dw.IsActive).HasDefaultValue(true);
			builder.Property(dw => dw.CreatedDate).IsRequired();
			builder.Property(dw => dw.UpdatedDate).IsRequired(false);
			builder.Property(dw => dw.DeletedDate).IsRequired(false);

			builder.Property(dw => dw.UserId).IsRequired();
			builder.Property(dw => dw.Balance).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0); // Başlangıç bakiyesi olarak 0
			builder.Property(dw => dw.Points).IsRequired().HasColumnType("decimal(18,2)").HasDefaultValue(0); // Başlangıç puanı olarak 0

			builder.HasOne(dw => dw.User)
			  .WithOne(u => u.DigitalWallet)
			  .HasForeignKey<DigitalWallet>(dw => dw.UserId)
			  .OnDelete(DeleteBehavior.Restrict);




		}
	}

}
