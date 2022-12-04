using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore7Demo.Data.Maps;

public class AddressMap : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address", "dbo");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.PersonId).IsRequired();
        builder.Property(c => c.StreetNumber).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.City).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.State).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.ZipCode).IsRequired().HasMaxLength(10).IsUnicode(false);
    }
}
