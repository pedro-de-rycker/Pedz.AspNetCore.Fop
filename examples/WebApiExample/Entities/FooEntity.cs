using Pedz.AspNetCore.Fop.MinimalApi.Attributes;
using System.ComponentModel.DataAnnotations;
using WebApiExample.Dtos;

namespace WebApiExample.Entities;

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