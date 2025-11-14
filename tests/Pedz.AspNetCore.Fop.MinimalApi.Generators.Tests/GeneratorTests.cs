using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Pedz.AspNetCore.Fop.MinimalApi.Generators.Tests;

public class GeneratorTests
{
    [Test]
    public async Task TestGenerator__ShouldBeSuccessfull()
    {
        // Create the 'input' compilation that the generator will act on
        Compilation inputCompilation = CreateCompilation(@"
        using System.ComponentModel.DataAnnotations;
        using Pedz.AspNetCore.Fop.MinimalApi.Generators.Attributes;

        namespace WebApiExample.Entities;

        // Give user power on namespace and name
        [OffsetFop]
        public partial class FooEntity
        {
            public int Id { get; set; }

            [FopProperty(IsFilterable = false, IsSortable = true)]
            public required string FooProperty { get; set; }

            public required string MyProperty { get; set; }
        }
        ");

        // directly create an instance of the generator
        // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
        FopGenerator generator = new FopGenerator();

        // Create the driver that will control the generation, passing in our generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the generation pass
        // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        if (outputCompilation.GetDiagnostics() is [_, ..] errors)
        {
            Console.WriteLine(errors.Select(x => x.GetMessage()));
        }

        await Assert.That(diagnostics.IsEmpty).IsTrue();
        await Assert.That(diagnostics.Count()).IsEqualTo(0);
        await Assert.That(outputCompilation.SyntaxTrees.Count()).IsEqualTo(11);

        // Or we can look at the results directly:
        GeneratorDriverRunResult runResult = driver.GetRunResult();

        // The runResult contains the combined results of all generators passed to the driver
        await Assert.That(runResult.GeneratedTrees.Length).IsEqualTo(10);
        await Assert.That(runResult.Diagnostics).IsEmpty();

        // Or you can access the individual results on a by-generator basis
        GeneratorRunResult generatorResult = runResult.Results[0];
        await Assert.That(generatorResult.Generator).IsEquivalentTo(generator);
        await Assert.That(generatorResult.Diagnostics).IsEmpty();
        await Assert.That(generatorResult.GeneratedSources.Length).IsEqualTo(10);
        await Assert.That(generatorResult.Exception).IsNull();
    }

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            [CSharpSyntaxTree.ParseText(source)],
            [MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location), .. Basic.Reference.Assemblies.Net90.References.All],
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}
