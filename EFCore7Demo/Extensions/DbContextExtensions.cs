using EFCore7Demo.Attributes;
using EFCore7Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore7Demo.Extensions;

public static class DbContextExtensions {
    /// <summary>
    /// Save any EntityEntry changes to the database.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="entry"></param>
    public static void SaveChangeLog(this DbContext context, EntityEntry entry) {
        var changeLog = CreateChangeLog(entry);
        if (changeLog != null) {
            context.Add(changeLog);
        }
    }

    /// <summary>
    /// Create change log for valid EntityEntry and changed properties.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static ChangeLog CreateChangeLog(EntityEntry entry) {
        if (entry.State is EntityState.Detached or EntityState.Unchanged or EntityState.Deleted)
            return null;

        if (entry.IsEntryExcluded())
            return null;

        var changeDetails = GetEntryChangeDetails(entry, new List<ChangeDetail>());
        if (!changeDetails.Any())
            return null;

        return new ChangeLog {
            TableName = entry.Metadata.GetTableName(),
            TableKey = entry.PrimaryKey(),
            ChangeDetails = changeDetails,
            ChangeType = entry.State.ToString(),
            ChangedOn = DateTime.Now
        };
    }

    /// <summary>
    /// Go through properties and navigations recursively to retrieve changed values.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="details"></param>
    /// <returns></returns>
    private static List<ChangeDetail> GetEntryChangeDetails(EntityEntry entry, List<ChangeDetail> details) {
        var validProperties = GetValidProperties(entry);
        foreach (var prop in validProperties) {
            // added properties
            if (entry.State == EntityState.Added) {
                details.Add(new ChangeDetail {
                    PropertyName = prop.Metadata.Name,
                    OldValue = null,
                    NewValue = prop.CurrentValue?.ToString()
                });
                continue;
            }
            // changed properties
            // TODO nested SQL objects are Modified state, but don't have the original value, why?
            if (prop.IsModified && prop.OriginalValue != null && !prop.OriginalValue.Equals(prop.CurrentValue))
                details.Add(new ChangeDetail {
                    PropertyName = prop.Metadata.Name,
                    OldValue = prop.OriginalValue?.ToString(),
                    NewValue = prop.CurrentValue?.ToString()
                });
        }

        return details;
    }

    /// <summary>
    /// Returns the primary key for the EntityEntry
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static int PrimaryKey(this EntityEntry entry) {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null)
            return 0;

        var values = key.Properties
            .Select(p => entry.Property(p.Name).CurrentValue)
            .Where(value => value != null)
            .ToList();

        if (!values.Any()) 
            return 0;
        if (values.Count > 1) 
            return -1;
        
        var isInt = int.TryParse(values[0].ToString(), out int result);
        return isInt ? result : -1;
    }

    /// <summary>
    /// Check if class has ExcludeFromChangeLog attribute.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static bool IsEntryExcluded(this EntityEntry entry) {
        return entry.Metadata.ClrType
            .GetCustomAttributes(typeof(ExcludeFromChangeLog), true).Any();
    }

    /// <summary>
    /// Get all entry properties without an ExcludeFromChangeLog attribute.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static IList<PropertyEntry> GetValidProperties(EntityEntry entry) {
        var excludedProperties = entry.Metadata.ClrType.GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(ExcludeFromChangeLog), true).Any())
            .Select(p => p.Name);
        return entry.Properties.Where(p => !excludedProperties.Contains(p.Metadata.Name)).ToList();
    }
}
