using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace EFCore7Demo.Data.Maps;

public class ChangeLogMap : IEntityTypeConfiguration<ChangeLog>
{
    public void Configure(EntityTypeBuilder<ChangeLog> builder)
    {
        builder.ToTable("ChangeLog", "dbo");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.TableName).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.TableKey).IsRequired();
        builder.Property(c => c.ChangeType).IsRequired().HasMaxLength(16).IsUnicode(false);
        builder.Property(c => c.ChangedOn).HasColumnType("datetime2").IsRequired();

        builder.Property(c => c.ChangeDetails).IsRequired().IsUnicode(true)
            .HasConversion(c => JsonSerializer.Serialize(c, (JsonSerializerOptions)null),
                            c => JsonSerializer.Deserialize<List<ChangeDetail>>(c, (JsonSerializerOptions) null));
    }
}
