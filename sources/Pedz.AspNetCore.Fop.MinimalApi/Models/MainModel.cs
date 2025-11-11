using Microsoft.CodeAnalysis;

namespace Pedz.AspNetCore.Fop.MinimalApi.Models;

internal class MainModel
{
    public required string Namespace { get; set; }

    public required string ClassName { get; set; }

    public required string OffsetQueryName { get; set; }

    public required string SortQueryName { get; set; }

    public List<(string name, string definedName)>? SortableProperties { get; set; }

    public required string FilterQueryPrefixName { get; set; }

    public List<(string name, string definedName, ITypeSymbol type)>? FilterableProperties { get; set; }

    public required int MaxSize { get; set; }
}
