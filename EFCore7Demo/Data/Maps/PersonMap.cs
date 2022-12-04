using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore7Demo.Data.Maps;

public class PersonMap : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Person", "dbo");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100).IsUnicode(false);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100).IsUnicode(false);

        builder.OwnsMany(p => p.Addresses, ab =>
        {
            ab.ToTable("Address", "dbo");
            ab.HasKey(a => a.Id);
            ab.Property(a => a.Id).ValueGeneratedOnAdd();
            ab.Property(a => a.PersonId).IsRequired();
            ab.Property(a => a.StreetNumber).IsRequired().HasMaxLength(100).IsUnicode(false);
            ab.Property(a => a.City).IsRequired().HasMaxLength(100).IsUnicode(false);
            ab.Property(a => a.State).IsRequired().HasMaxLength(100).IsUnicode(false);
            ab.Property(a => a.ZipCode).IsRequired().HasMaxLength(10).IsUnicode(false);
        });
    }
}
