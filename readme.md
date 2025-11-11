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
```
?offset[500]=1000
?sort-by=asc:fooProperty
?properties.fooProperty=eq:barValue
```