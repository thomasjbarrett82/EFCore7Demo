using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore7Demo.Data.Maps;

public class PersonJsonMap : IEntityTypeConfiguration<PersonJson>
{
    public void Configure(EntityTypeBuilder<PersonJson> builder)
    {
        builder.ToTable("PersonJson", "dbo");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100).IsUnicode(false);

        builder.OwnsMany(p => p.Addresses, ab =>
        {
            ab.ToJson();
        });
    }
}
