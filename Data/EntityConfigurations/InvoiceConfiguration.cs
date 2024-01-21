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
        builder.Property(invoice => invoice.Note).HasMaxLength(255);
    }
}