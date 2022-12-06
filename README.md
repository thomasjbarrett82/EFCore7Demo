# EFCore7Demo

Demonstration of Entity Framework Core 7.0 ChangeTracker behavior for nested objects, both using SQL tables and the new JSON features. The goal is a ChangeLogInterceptor that only records the added and changed values for nested objects.

Original version was not using the Update() method correctly, see [this GitHub issue](https://github.com/dotnet/efcore/issues/29765) and the repo history for more information.
