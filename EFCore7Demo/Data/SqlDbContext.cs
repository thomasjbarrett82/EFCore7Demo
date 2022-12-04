using EFCore7Demo.Data.Maps;
using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore7Demo.Data;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonJson> PersonJsons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ChangeLog> ChangeLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonMap());
        modelBuilder.ApplyConfiguration(new PersonJsonMap());
        modelBuilder.ApplyConfiguration(new ChangeLogMap());
    }
}
