using Microsoft.EntityFrameworkCore.Metadata.Builders;
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


			builder.HasMany(u => u.Baskets)  // Kullanıcı ve sepet arasında bir-çok ilişki tanımlandı.
		  .WithOne(b => b.User)
		  .HasForeignKey(b => b.UserId)
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
