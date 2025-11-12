namespace Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests;

public class TestsSelection
{
    //[Test]
    //public async Task MyTest()
    //{
    //    // Create the 'input' compilation that the generator will act on
    //    Compilation inputCompilation = CreateCompilation(@"
    //    using System.ComponentModel.DataAnnotations;
    //    using Pedz.AspNetCore.Fop.MinimalApi.Generators.Attributes;

    //    namespace WebApiExample.Entities;

    //    // Give user power on namespace and name
    //    [OffsetFop]
    //    public partial class FooEntity
    //    {
    //        public int Id { get; set; }

    //        [FopProperty(IsFilterable = false, IsSortable = true)]
    //        public required string FooProperty { get; set; }

    //        public required string MyProperty { get; set; }
    //    }
    //    ");

    //    // directly create an instance of the generator
    //    // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
    //    FopGenerator generator = new FopGenerator();

    //    // Create the driver that will control the generation, passing in our generator
    //    GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

    //    // Run the generation pass
    //    // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
    //    driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

    //    if (outputCompilation.GetDiagnostics() is [_, ..] errors)
    //    {
    //        Console.WriteLine(errors.Select(x => x.GetMessage()));
    //    }

    //    await Assert.That(diagnostics.IsEmpty).IsTrue();
    //    await Assert.That(outputCompilation.GetDiagnostics().Count()).IsEqualTo(0);
    //    await Assert.That(outputCompilation.SyntaxTrees.Count()).IsEqualTo(2);
    //    //Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
    //    //Debug.Assert(outputCompilation.SyntaxTrees.Count() == 2); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
    //    //Debug.Assert(outputCompilation.GetDiagnostics().IsEmpty); // verify the compilation with the added source has no diagnostics

    //    // Or we can look at the results directly:
    //    GeneratorDriverRunResult runResult = driver.GetRunResult();

    //    // The runResult contains the combined results of all generators passed to the driver
    //    //Debug.Assert(runResult.GeneratedTrees.Length == 1);
    //    //Debug.Assert(runResult.Diagnostics.IsEmpty);

    //    // Or you can access the individual results on a by-generator basis
    //    GeneratorRunResult generatorResult = runResult.Results[0];
    //    //Debug.Assert(generatorResult.Generator == generator);
    //    //Debug.Assert(generatorResult.Diagnostics.IsEmpty);
    //    //Debug.Assert(generatorResult.GeneratedSources.Length == 1);
    //    //Debug.Assert(generatorResult.Exception is null);
    //}

    //private static Compilation CreateCompilation(string source)
    //        => CSharpCompilation.Create("compilation",
    //            [CSharpSyntaxTree.ParseText(source)],
    //            [MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location), .. Basic.Reference.Assemblies.Net90.References.All],
    //            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}
