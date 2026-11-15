using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FlashOWare.CodeAnalysis.Generators.CSharp.Roslyn;

#if ROSLYN4_0_OR_GREATER
[Generator(LanguageNames.CSharp)]
public sealed class PlaceholderGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(static (IncrementalGeneratorPostInitializationContext context) =>
		{
			string source = """
				namespace Generated
				{
					public sealed class Placeholder
					{
						private readonly int number;

						public Placeholder(int number)
						{
							this.number = number;
						}

						public override string ToString() => $"{number}";
					}
				}
				""";
			context.AddSource("Placeholder.g.cs", SourceText.From(source, Encoding.UTF8));
		});
	}
}
#else
[Generator(LanguageNames.CSharp)]
public sealed class PlaceholderGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForPostInitialization(static (GeneratorPostInitializationContext context) =>
		{
			string source = """
				namespace Generated
				{
					public sealed class Placeholder
					{
						private readonly int number;

						public Placeholder(int number)
						{
							this.number = number;
						}

						public override string ToString() => $"{number}";
					}
				}
				""";
			context.AddSource("Placeholder.g.cs", SourceText.From(source, Encoding.UTF8));
		});
	}

	public void Execute(GeneratorExecutionContext context)
	{
	}
}
#endif
