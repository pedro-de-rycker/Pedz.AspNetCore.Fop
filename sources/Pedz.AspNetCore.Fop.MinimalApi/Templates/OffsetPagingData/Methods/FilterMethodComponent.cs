using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis.Lyra;
using static RhoMicro.CodeAnalysis.Lyra.ComponentFactory;

namespace Pedz.AspNetCore.Fop.MinimalApi.Templates.OffsetPagingData.Methods;

internal class FilterMethodComponent(
    string targetedEntityNamespace,
    string targetedEntityClassName,
    List<(string name, string definedName, ITypeSymbol type)>? filterableProperties)
    : ICSharpSourceComponent
{
    public void AppendTo(
        CSharpSourceBuilder builder,
        CancellationToken cancellationToken = default)
    {
        var queryableType = TypeName($"global::System.Linq.IQueryable<{targetedEntityNamespace}.{targetedEntityClassName}>");

        var equalityLoop = Create(
            (filterableProperties, targetedEntityClassName),
            static (m, b, ct) =>
            {
                if (m.filterableProperties is not null)
                    foreach (var (name, definedName, type) in m.filterableProperties)
                    {
                        var fullyQualifiedName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                        b.AppendLine(
                            $$"""
                            // fullyQualifiedName: {{fullyQualifiedName}}
                            if (name.Equals("{{definedName}}", global::System.StringComparison.InvariantCultureIgnoreCase))
                            {
                            """)
                            .Indent();

                        if (type.SpecialType == SpecialType.System_Int32)
                        {
                            b.AppendLine(
                                $$"""
                                if (int.TryParse(value, out var intValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == intValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != intValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > intValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= intValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < intValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= intValue);
                                    }
                                }
                                """);
                        }
                        else if (type.SpecialType == SpecialType.System_String)
                        {
                            b.AppendLine(
                                $$"""
                                if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                {
                                    return query.Where(f => f.{{name}} == value);
                                }
                                else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                {
                                    return query.Where(f => f.{{name}} != value);
                                }
                                else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                {
                                    return query.Where(f => string.Compare(f.{{name}}, value) > 0);
                                }
                                else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                {
                                    return query.Where(f => string.Compare(f.{{name}}, value) >= 0);
                                }
                                else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                {
                                    return query.Where(f => string.Compare(f.{{name}}, value) < 0);
                                }
                                else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                {
                                    return query.Where(f => string.Compare(f.{{name}}, value) <= 0);
                                }
                                """);
                        }
                        else if (fullyQualifiedName == "Guid")
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.Guid.TryParse(value, out var guidValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == guidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != guidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > guidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= guidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < guidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= guidValue);
                                    }
                                }
                                """);
                        }
                        else if (fullyQualifiedName == "Guid?")
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.Guid?.TryParse(value, out var nullableGuidValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == nullableGuidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != nullableGuidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > nullableGuidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= nullableGuidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < nullableGuidValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= nullableGuidValue);
                                    }
                                }
                                """);
                        }
                        else if (type.SpecialType == SpecialType.System_DateTime)
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.DateTime.TryParse(value, out var dateTimeValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == dateTimeValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != dateTimeValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > dateTimeValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= dateTimeValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < dateTimeValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= dateTimeValue);
                                    }
                                }
                                """);
                        }
                        else if (fullyQualifiedName is "DateTimeOffset" or "global::System.DateTimeOffset")
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.DateTimeOffset.TryParse(value, out var dateTimeOffsetValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == dateTimeOffsetValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != dateTimeOffsetValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > dateTimeOffsetValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= dateTimeOffsetValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < dateTimeOffsetValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= dateTimeOffsetValue);
                                    }
                                }
                                """);
                        }
                        else if (fullyQualifiedName is "DateOnly" or "global::System.DateOnly")
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.DateOnly.TryParse(value, out var dateOnlyValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == dateOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != dateOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > dateOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= dateOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < dateOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= dateOnlyValue);
                                    }
                                }
                                """);
                        }
                        else if (fullyQualifiedName is "TimeOnly" or "global::System.TimeOnly")
                        {
                            b.AppendLine(
                                $$"""
                                if (global::System.TimeOnly.TryParse(value, out var timeOnlyValue))
                                {
                                    if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal)
                                    {
                                        return query.Where(f => f.{{name}} == timeOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual)
                                    {
                                        return query.Where(f => f.{{name}} != timeOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior)
                                    {
                                        return query.Where(f => f.{{name}} > timeOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} >= timeOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior)
                                    {
                                        return query.Where(f => f.{{name}} < timeOnlyValue);
                                    }
                                    else if (filteringType == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual)
                                    {
                                        return query.Where(f => f.{{name}} <= timeOnlyValue);
                                    }
                                }
                                """);
                        }

                        b.Detent()
                            .AppendLine(
                                $$"""
                                }
                                """);
                    }
            });

        builder
            .AppendLine(
                $$"""
                private static {{queryableType}} FilterProperty(
                    string name,
                    string value,
                    global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum filteringType,
                    {{queryableType}} query)
                {
                    {{equalityLoop}}
                    
                    throw new global::System.NotImplementedException($"This case is not supported for filtering.");
                }

                public override {{queryableType}} Filter(
                    {{queryableType}} query)
                {
                    foreach (var (name, value, filteringType) in this.FilterBy)
                    {
                        query = FilterProperty(name, value, filteringType, query);
                    }
                    return query;
                }
                """);
    }
}
