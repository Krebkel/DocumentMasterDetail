using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(invoice => invoice.Id);
        builder.HasIndex(invoice => invoice.Number).IsUnique();
        builder.Property(invoice => invoice.Date).IsRequired();
        builder.Property(invoice => invoice.TotalAmount).IsRequired();
        builder.Property(invoice => invoice.Note).HasMaxLength(255);

        // Определение связи "один ко многим" с сущностью Position
        builder.HasMany(invoice => invoice.Positions)
            .WithOne(position => position.Invoice)
            .OnDelete(DeleteBehavior.Cascade);
    }
}