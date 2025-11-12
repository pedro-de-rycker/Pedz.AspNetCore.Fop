using Pedz.AspNetCore.Fop.MinimalApi.Attributes;

namespace Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests.Entities;

[OffsetFop]
public class OffsetEntity
{
    [FopProperty(IsSortable = true, IsFilterable = false)]
    public Guid Id { get; set; }
}
