using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Theks.Order.Infrastructure.Data.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{

    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        // @Hint: Convert Guid to byte[] when saving, convert byte[] to Guid when reading
        builder.Property(p => p.Id)
            .HasConversion(
                x => x.ToByteArray(),
                y => new Guid(y)
        );

        builder.Property(p => p.ProductId)
            .HasConversion(
                x => x.ToByteArray(),
                y => new Guid(y)
        );

        builder.Property(p => p.ClientId)
            .HasConversion(
                x => x.ToByteArray(),
                y => new Guid(y)
        );

        builder.Property(p => p.Quantity)
        .IsRequired();

        builder.Property(p => p.CreatedDate)
            .IsRequired();
    }
}