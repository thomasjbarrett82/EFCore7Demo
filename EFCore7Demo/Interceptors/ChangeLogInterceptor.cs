using EFCore7Demo.Extensions;
using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore7Demo.Interceptors
{
    public class ChangeLogInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var p = dbContext.ChangeTracker.Entries<Person>();
            foreach (var entry in p.ToList())
            {
                dbContext.SaveChangeLog(entry);
            }

            var a = dbContext.ChangeTracker.Entries<Address>();
            foreach (var entry in a.ToList()) {
                dbContext.SaveChangeLog(entry);
            }

            var pj = dbContext.ChangeTracker.Entries<PersonJson>();
            foreach (var entry in pj.ToList()) {
                dbContext.SaveChangeLog(entry);
            }

            var aj = dbContext.ChangeTracker.Entries<AddressJson>();
            foreach (var entry in aj.ToList()) {
                dbContext.SaveChangeLog(entry);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
