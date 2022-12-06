# EFCore7Demo

Demonstration of Entity Framework Core 7.0 ChangeTracker behavior for nested objects, both using SQL tables and the new JSON features. The goal is a ChangeLogInterceptor that only records the added and changed values for nested objects.

Original version was not using the Update() method correctly, see [GetEntryChangeDetails method in DbContextExtensions.cs, line 52](https://github.com/thomasjbarrett82/EFCore7Demo/blob/master/EFCore7Demo/Extensions/DbContextExtensions.cs#L52) and this repo history for more information.
