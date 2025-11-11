using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Pedz.AspNetCore.Fop.MinimalApi;
using System.Diagnostics;

namespace tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Create the 'input' compilation that the generator will act on
        Compilation inputCompilation = CreateCompilation(@"
        using Pedz.AspNetCore.Fop.MinimalApi.Attributes;

        namespace WebApiExample.Entities;

        [OffsetFop]
        public class FooEntity
        {
            public int Id { get; set; }

            [FopProperty(IsFilterable = true, IsSortable = true)]
            public string FooProperty { get; set; }

            [FopProperty(IsFilterable = true, IsSortable = true)]
            public Guid? GuidProperty { get; set; }

            [FopProperty(IsFilterable = true, IsSortable = true)]
            public DateTimeOffset DateTimeOffsetProperty { get; set; }

            public string MyProperty { get; set; }
        }
        ");

        // directly create an instance of the generator
        // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
        FopGenerator generator = new FopGenerator();

        // Create the driver that will control the generation, passing in our generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the generation pass
        // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics, TestContext.Current.CancellationToken);

        if (outputCompilation?.GetDiagnostics() is [_, ..] errors)
        {
            TestContext.Current.TestOutputHelper?.WriteLine(string.Join(',', errors.Select(x => x.GetMessage())));
        }

        var errorDiagnostics = outputCompilation.GetDiagnostics(TestContext.Current.CancellationToken).Where(x => x.Severity == DiagnosticSeverity.Error);

        Assert.Empty(errorDiagnostics);
        Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
        Debug.Assert(outputCompilation.GetDiagnostics(TestContext.Current.CancellationToken).IsEmpty); // verify the compilation with the added source has no diagnostics
        Debug.Assert(outputCompilation.SyntaxTrees.Count() == 4); // we have two syntax trees, the original 'user' provided one, and the one added by the generator

        // Or we can look at the results directly:
        GeneratorDriverRunResult runResult = driver.GetRunResult();

        // The runResult contains the combined results of all generators passed to the driver
        Debug.Assert(runResult.GeneratedTrees.Length == 3);
        Debug.Assert(runResult.Diagnostics.IsEmpty);

        // Or you can access the individual results on a by-generator basis
        GeneratorRunResult generatorResult = runResult.Results[0];
        Debug.Assert(generatorResult.Generator == generator);
        Debug.Assert(generatorResult.Diagnostics.IsEmpty);
        Debug.Assert(generatorResult.GeneratedSources.Length == 1);
        Debug.Assert(generatorResult.Exception is null);
    }

    private static Compilation CreateCompilation(string source) => CSharpCompilation.Create("compilation",
        [CSharpSyntaxTree.ParseText(source)],
        [.. Basic.Reference.Assemblies.NetStandard20.References.All],
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, reportSuppressedDiagnostics: false));
}
