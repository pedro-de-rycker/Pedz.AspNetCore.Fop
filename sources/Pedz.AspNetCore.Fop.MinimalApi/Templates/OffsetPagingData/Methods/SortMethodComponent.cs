using RhoMicro.CodeAnalysis.Lyra;
using static RhoMicro.CodeAnalysis.Lyra.ComponentFactory;

namespace Pedz.AspNetCore.Fop.MinimalApi.Templates.OffsetPagingData.Methods;

internal class SortMethodComponent(
    string targetedEntityNamespace,
    string targetedEntityClassName,
    List<(string name, string definedName)>? sortableProperties)
    : ICSharpSourceComponent
{
    public void AppendTo(
        CSharpSourceBuilder builder,
        CancellationToken cancellationToken = default)
    {
        var type = $"{targetedEntityNamespace}.{targetedEntityClassName}";
        var queryableType = TypeName($"global::System.Linq.IQueryable<{type}>");

        var loop = Create(
            (sortableProperties, type),
            static (m, b, ct) =>
            {
                if (m.sortableProperties is not null)
                    foreach (var (name, definedName) in m.sortableProperties)
                    {
                        b.Append(
                        $$"""
                        if (name.Equals("{{definedName}}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return SortDirection == global::Pedz.AspNetCore.Fop.MinimalApi.Enums.SortDirectionEnum.Ascending ? query.OrderBy(f => f.{{name}}) : query.OrderByDescending(f => f.{{name}});
                        }

                        """);
                    }
            });

        builder
            .Append(
                $$"""
                public override {{queryableType}} Sort(
                    string name,
                    {{queryableType}} query)
                {
                    {{loop}}

                    throw new global::System.NotImplementedException($"Property doesn't exist or is not sortable.");
                }

                """);
    }
}
