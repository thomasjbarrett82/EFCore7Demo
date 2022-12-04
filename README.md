# EFCore7Demo

Demonstration of Entity Framework Core 7.0 ChangeTracker behavior for nested objects, both using SQL tables and the new JSON features. The goal is a ChangeLogInterceptor that only records the added and changed values for nested objects.

## Models

```csharp
public class Person {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Address> Addresses { get; set; }
}

public class PersonJson {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<AddressJson> Addresses { get; set; }
}

public class Address { 
    public int Id { get; set; }
    public string StreetNumber { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public int PersonId { get; set; }
}

public class AddressJson {
    public string StreetNumber { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

public class ChangeLog {
    public int Id { get; set; }
    public string TableName { get; set; }
    public int TableKey { get; set; }
    public List<ChangeDetail> ChangeDetails { get; set; }
    public string ChangeType { get; set; }
    public DateTime ChangedOn { get; set; }
}

public class ChangeDetail {
    public string PropertyName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}
```

## Test Cases & Results

### Add a new Person

1. SQL: works as expected.
2. JSON: works as expected.

### Add a new Person + Address

1. SQL: works as expected.
2. JSON: works as expected.

### Add a new Address

1. SQL: works as expected.
2. JSON: **Behavior Not Expected:** Both the existing and new Address.State == Added, only expect the new Address in this state.

### Modify a Person

1. SQL: works as expected.
2. JSON: **Behavior Not Expected:** Person.State == Modified, which is expected, but then also Address.State == Added, even if no changes were made to the Address.

### Modify an Address

1. SQL: **Behavior Not Expected:** Address.State == Modified, but OriginalValue == CurrentValue, so not possible to extract only changed values.
2. JSON: **Behavior Not Expected:** Address.State == Added, expected Modified.
