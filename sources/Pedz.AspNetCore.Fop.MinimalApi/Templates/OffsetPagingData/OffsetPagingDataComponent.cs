using Pedz.AspNetCore.Fop.MinimalApi.Models;
using Pedz.AspNetCore.Fop.MinimalApi.Templates.OffsetPagingData.Methods;
using RhoMicro.CodeAnalysis.Lyra;
using static RhoMicro.CodeAnalysis.Lyra.ComponentFactory;

namespace Pedz.AspNetCore.Fop.MinimalApi.Templates.OffsetPagingData;

internal class OffsetPagingDataComponent(
    MainModel model)
    : ICSharpSourceComponent
{
    public void AppendTo(
        CSharpSourceBuilder builder,
        CancellationToken cancellationToken = default)
    {
        var dataClassName = $"{model.ClassName}OffsetPagingData";

        var getPaginationData = Create(
            (dataClassName, model),
            static (m, b, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                b.AppendLine(
                    $$"""
                    public static (int size, int page) GetPaginationData(
                        global::Microsoft.AspNetCore.Http.HttpContext context)
                    {
                        return BindAsync(context, parameter: null!);
                    }
                    """);
            });

        var body = Create(
            (dataClassName, model),
            static (m, b, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                b.AppendLine(
                    $$"""
                    private static global::System.Text.RegularExpressions.Regex OffsetValueRegex =
                        new(pattern: "^{{m.model.OffsetQueryName}}(?<size>\\[\\d+\\])?$",
                            options: global::System.Text.RegularExpressions.RegexOptions.Compiled | global::System.Text.RegularExpressions.RegexOptions.IgnoreCase,
                            matchTimeout: global::System.TimeSpan.FromMilliseconds(200));

                    {{new OffsetRetreiveMethodeComponent(m.model.Namespace, m.model.ClassName, m.model.MaxSize)}}
                    {{new SortMethodComponent(m.model.Namespace, m.model.ClassName, m.model.SortableProperties)}}
                    {{new FilterMethodComponent(m.model.Namespace, m.model.ClassName, m.model.FilterableProperties)}}

                    public static ValueTask<{{m.dataClassName}}?> BindAsync(
                        global::Microsoft.AspNetCore.Http.HttpContext context, 
                        global::System.Reflection.ParameterInfo parameter)
                    {
                        // Offset
                        const string currentPageKey = "{{m.model.OffsetQueryName}}";
                        var offsetingProperties = context.Request.Query
                            .Where(q => q.Key.StartsWith(currentPageKey, global::System.StringComparison.InvariantCultureIgnoreCase));
                        var offsetingProperty = offsetingProperties.FirstOrDefault();
                        var size = {{m.model.MaxSize}};
                        var offsetValue = offsetingProperty.Value.ToString();
                        var offset = 0;

                        if (offsetValue is not null && int.TryParse(offsetValue, out int parsedOffset))
                            offset = parsedOffset;

                        if (offsetingProperty.Key is not null)
                        {
                            var match = OffsetValueRegex.Match(offsetingProperty.Key);
                            if (match.Success)
                            {
                                var sizeGroup = match.Groups["size"];
                                if (sizeGroup.Success)
                                {
                                    var sizeValue = sizeGroup.Value.Trim('[', ']');
                                    if (int.TryParse(sizeValue, out var parsedSize))
                                        size = parsedSize;
                                }
                            }
                        }

                        // Sorting
                        const string sortByKey = "{{m.model.SortQueryName}}";
                        var sortDirection = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.SortDirectionEnum.Ascending;
                        var sortBy = context.Request.Query[sortByKey].FirstOrDefault();

                        if (sortBy is not null)
                        {
                            var workedSortBy = sortBy.Split(':');

                            if (workedSortBy.Length == 2 && workedSortBy.First() is "desc")
                                sortDirection = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.SortDirectionEnum.Descending;

                            sortBy = workedSortBy.Last();
                        }

                        // Filtering
                        var filteringPrefix = "{{m.model.FilterQueryPrefixName}}.";
                        var filteringProperties = context.Request.Query
                            .Where(q => q.Key.StartsWith(filteringPrefix, global::System.StringComparison.InvariantCultureIgnoreCase));

                        global::System.Collections.Generic.List<(string name, string value, global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum filteringType)> filterBy = [];

                        foreach (var filteringProperty in filteringProperties)
                        {
                            var filteredPropertyKey = filteringProperty.Key.Substring(filteringPrefix.Length);
                            var filteringTypeValues = filteringProperty.Value.ToString().Split(':');
                            var filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal;
                            string? filteringTypeValue = null;

                            if (filteringTypeValues.Length >= 2)
                            {
                                filteringTypeValue = filteringTypeValues.First();
                                if (filteringTypeValue is "eq")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Equal;
                                }
                                else if (filteringTypeValue is "neq")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.NotEqual;
                                }
                                else if (filteringTypeValue is "gt")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Superior;
                                }
                                else if (filteringTypeValue is "gteq")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.SuperiorOrEqual;
                                }
                                else if (filteringTypeValue is "lt")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.Inferior;
                                }
                                else if (filteringTypeValue is "lteq")
                                {
                                    filteringType = global::Pedz.AspNetCore.Fop.MinimalApi.Enums.FilteringTypeEnum.InferiorOrEqual;
                                }

                                filteringTypeValue += ":";
                            }
                    
                            var filteredValue = filteringProperty.Value.ToString().Substring(filteringTypeValue?.Length ?? 0);
                            filterBy.Add((filteredPropertyKey, filteredValue, filteringType));
                        }

                        var result = new {{m.dataClassName}}
                        {
                            SortBy = sortBy,
                            FilterBy = filterBy,
                            SortDirection = sortDirection,
                            Offset = offset,
                            Size = size,
                        };

                        return ValueTask.FromResult<{{m.dataClassName}}?>(result);
                    }
                    """);
            });

        var type = Type(
            "public class",
            dataClassName,
            body,
            baseTypeList: [TypeName($"global::Pedz.AspNetCore.Fop.MinimalApi.Attributes.OffsetPagingData<{model.Namespace}.{model.ClassName}>")]
            );

        var namespaces = Namespace(
            model.Namespace,
            type);

        builder.AppendLine(namespaces);
    }
}
