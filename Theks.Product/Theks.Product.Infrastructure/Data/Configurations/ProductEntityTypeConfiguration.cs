using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Theks.Product.Infrastructure.Data.Configurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Product>
{

    public void Configure(EntityTypeBuilder<Domain.Entities.Product> builder)
    {
        // @Hint: Convert Guid to byte[] when saving, convert byte[] to Guid when reading
        builder.Property(p => p.Id)
            .HasConversion(
                x => x.ToByteArray(),
                y => new Guid(y)
        );

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Description)
        .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .IsRequired();
    }
}