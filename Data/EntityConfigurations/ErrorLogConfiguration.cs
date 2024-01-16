using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

internal class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.HasKey(errorLog => errorLog.Id);

        builder.Property(errorLog => errorLog.Date).IsRequired();
        builder.Property(errorLog => errorLog.Note).IsRequired();
    }
}