# AsNoTracking

We should use `.AsNoTracking()` in any Entity Framework query to retrieve readonly data. This will ensure minimal memory usage and optimal performance.

```csharp
public static void AsNoTrackingQuery()
{
    using var dbContext = new MyDbContext();
    _ = dbContext.Todos.AsNoTracking().ToList();
    var trackingCount = dbContext.ChangeTracker.Entries().Count();
    Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
    // output: 0 ChangeTracker Entries.
}

public static void AsQueryableQuery()
{
    using var dbContext = new MyDbContext();
    _ = dbContext.Todos.AsQueryable().ToList();
    var trackingCount = dbContext.ChangeTracker.Entries().Count();
    Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
    // output: 100000 ChangeTracker Entries.
}

public static void AsEnumerableQuery()
{
    using var dbContext = new MyDbContext();
    _ = dbContext.Todos.AsQueryable().ToList();
    var trackingCount = dbContext.ChangeTracker.Entries().Count();
    Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
    // output: 100000 ChangeTracker Entries.
}
```
