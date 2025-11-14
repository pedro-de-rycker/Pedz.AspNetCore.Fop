# Pedz.AspNetCore.Fop

> This package is highly experimental and should not be used in production environments.
> It is provided as-is without any warranties or support.
>
> Breaking changes may occur in future releases.

## Features

- Paginate based on offset and limit
- Sort by a property in ascending or descending order
- Filter by property values using different operators

## Installation

```
dotnet add package Pedz.AspNetCore.Fop.MinimalApi
```

## How To Use

Add a `[OffsetFop]` attribute on the entity class you want to enable FOP (Filter, Order, Pagination) for.

Then add `[FopProperty]` attributes on the properties you want to be filterable and/or sortable.

Example :

```csharp
[OffsetFop(MaxSize = 500, OffsetParameterPrefixName = "offset", PropertiesParameterPrefixName = "properties", SortParameterPrefixName = "sort-by")]
public class FooEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [FopProperty(IsFilterable = true, IsSortable = true)]
    public required string FooProperty { get; set; }

    [Required]
    [FopProperty(IsFilterable = false, IsSortable = true)]
    public required string MyProperty { get; set; }

    [Required]
    [FopProperty(IsFilterable = true, IsSortable = true, PropertyName = nameof(FooDto.Test))] // If you need to match dto property name
    public required int TestProperty { get; set; }

    [Required]
    [FopProperty(IsFilterable = true, IsSortable = true)]
    public required DateTimeOffset DateTimeOffsetProperty { get; set; }

    [Required]
    [FopProperty(IsFilterable = true, IsSortable = true)]
    public required DateOnly DateOnlyProperty { get; set; }

    [Required]
    [FopProperty(IsFilterable = true, IsSortable = true)]
    public required TimeOnly TimeOnlyProperty { get; set; }
}
```
This will generate a `OffsetPagingData` class in the same namespace as the targeted entity you can just place in the minimal api lambda and the binding will happen.

Example :
```csharp
app.MapGet("/test", async (
    HttpContext context,
    [FromServices] FooDbContext dbContext,
    FooEntityOffsetPagingData data) =>
{
    var foos = await dbContext.Foos
        .ApplyFop(data, query => query.OrderBy(x => x.Id))
        .ToListAsync();

    return Results.Ok(foos);
});
```

Example of possible calls :

```
?offset[500]=1000
?sort-by=asc:fooProperty
?properties.fooProperty=eq:barValue
```

## Supporting sort operators

The operators are not case sensitive.

- asc (ascending) **(default)**
- desc (descending)

## Supported filter operators

The operators are not case sensitive.

- eq (equal) **(default)**
- neq (not equal)
- gt (greater than)
- gteq (greater than or equal)
- lt (less than)
- lteq (less than or equal)