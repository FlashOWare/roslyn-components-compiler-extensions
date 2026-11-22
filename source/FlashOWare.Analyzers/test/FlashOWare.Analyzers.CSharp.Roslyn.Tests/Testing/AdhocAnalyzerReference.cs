using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests.Testing;

internal sealed class AdhocAnalyzerReference : AnalyzerReference
{
	private readonly string _display;
	private readonly object _id;

	private readonly DiagnosticAnalyzer? _analyzer;
	private readonly ISourceGenerator? _generator;

	private AdhocAnalyzerReference(Type type)
	{
		string name = type.ToString();
		Version? version = type.Assembly.GetName().Version;

		_display = name;
		_id = new AssemblyIdentity(name, version);
	}

	public AdhocAnalyzerReference(DiagnosticAnalyzer analyzer)
		: this(analyzer.GetType())
	{
		_analyzer = analyzer;
	}

	public AdhocAnalyzerReference(ISourceGenerator generator)
		: this(generator.GetType())
	{
		_generator = generator;
	}

#if !ROSLYN3_11
	public AdhocAnalyzerReference(IIncrementalGenerator generator)
		: this(generator.GetType())
	{
		_generator = generator.AsSourceGenerator();
	}
#endif

	public override string? FullPath => null;

	public override string Display => _display;

	public override object Id => _id;

	public override ImmutableArray<DiagnosticAnalyzer> GetAnalyzersForAllLanguages()
	{
		if (_analyzer is null)
		{
			return ImmutableArray<DiagnosticAnalyzer>.Empty;
		}

		return ImmutableArray.Create(_analyzer);
	}

	public override ImmutableArray<DiagnosticAnalyzer> GetAnalyzers(string language)
	{
		if (_analyzer is null)
		{
			return ImmutableArray<DiagnosticAnalyzer>.Empty;
		}

		if (_analyzer.GetType().GetCustomAttribute<DiagnosticAnalyzerAttribute>() is not { } attribute || !attribute.Languages.Contains(language, StringComparer.Ordinal))
		{
			return ImmutableArray<DiagnosticAnalyzer>.Empty;
		}

		return ImmutableArray.Create(_analyzer);
	}

	public override ImmutableArray<ISourceGenerator> GetGeneratorsForAllLanguages()
	{
		if (_generator is null)
		{
			return ImmutableArray<ISourceGenerator>.Empty;
		}

		return ImmutableArray.Create(_generator);
	}

	[Obsolete("Use GetGenerators(string language) or GetGeneratorsForAllLanguages()")]
	public override ImmutableArray<ISourceGenerator> GetGenerators()
	{
		throw new NotSupportedException("Use GetGenerators(string language) or GetGeneratorsForAllLanguages()");
	}

	public override ImmutableArray<ISourceGenerator> GetGenerators(string language)
	{
		if (_generator is null)
		{
			return ImmutableArray<ISourceGenerator>.Empty;
		}

#if !ROSLYN3_11
		if (_generator.GetGeneratorType().GetCustomAttribute<GeneratorAttribute>() is not { } attribute || !attribute.Languages.Contains(language, StringComparer.Ordinal))
		{
			return ImmutableArray<ISourceGenerator>.Empty;
		}
#endif

		return ImmutableArray.Create(_generator);
	}
}
