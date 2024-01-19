using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

internal class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(position => position.Id);

        builder.Property(position => position.Name).IsRequired();
        builder.Property(position => position.Quantity).IsRequired();
        builder.Property(position => position.Value).IsRequired();

        builder.HasOne(position => position.Invoice)
            .WithMany()
            .IsRequired();
    }
}