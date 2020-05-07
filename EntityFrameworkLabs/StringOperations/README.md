# EntityFramework Core String Operations

I am interested in the case sensitivity of string operations, such as `string.StartsWith`, `string.Contains`, `string.Equals`, and the `LIKE` operator in EF Core functions.

The result is very interesting. The translated SQL queries show that `string.Contains` and `string.Equals` are case sensitive, while `string.StartsWith` and `LIKE` are case insensitive.
