using RhoMicro.CodeAnalysis.Lyra;
using static RhoMicro.CodeAnalysis.Lyra.ComponentFactory;


namespace Pedz.AspNetCore.Fop.MinimalApi.Templates.OffsetPagingData.Methods;

internal class OffsetRetreiveMethodeComponent(
    string targetedEntityNamespace,
    string targetedEntityClassName,
    int maxPageSize)
    : ICSharpSourceComponent
{
    public void AppendTo(
        CSharpSourceBuilder builder,
        CancellationToken cancellationToken = default)
    {
        var queryableType = TypeName($"global::System.Linq.IQueryable<{targetedEntityNamespace}.{targetedEntityClassName}>");

        builder
            .Append(
                $$"""
                public override {{queryableType}} Retreive(
                    {{queryableType}} query)
                {
                    var workedSize = this.Size;
                    if (workedSize > {{maxPageSize}})
                        workedSize = {{maxPageSize}};
                    query = query.Skip(this.Offset).Take(workedSize);
                    return query;
                }

                """);
    }
}
