using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Papara.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Repository.EntityConfigurations
{
	public class StockConfiguration : IEntityTypeConfiguration<Stock>
	{
		public void Configure(EntityTypeBuilder<Stock> builder)
		{

			builder.Property(S => S.IsActive).HasDefaultValue(true);
			builder.Property(S => S.CreatedDate).IsRequired();
			builder.Property(S => S.UpdatedDate).IsRequired(false);
			builder.Property(S => S.DeletedDate).IsRequired(false);


			builder.Property(s => s.Quantity).IsRequired();

			builder.HasOne(s => s.Product)
				   .WithOne(p => p.ProductStock)
				   .HasForeignKey<Stock>(s => s.ProductId);



		}
	}
}
