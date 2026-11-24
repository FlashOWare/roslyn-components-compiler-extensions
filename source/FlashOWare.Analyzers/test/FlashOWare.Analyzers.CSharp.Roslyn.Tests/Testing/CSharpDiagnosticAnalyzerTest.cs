using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests.Testing;

internal sealed class CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> : CSharpAnalyzerTest<TDiagnosticAnalyzer, DefaultVerifier>
	where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
{
	private List<KeyValuePair<string, string>>? _features;

	public CSharpDiagnosticAnalyzerTest()
	{
	}

	protected override CompilationOptions CreateCompilationOptions()
	{
		var options = (CSharpCompilationOptions)base.CreateCompilationOptions();

		options = options
			.WithNullableContextOptions(NullableContextOptions.Enable);

		return options;
	}

	protected override ParseOptions CreateParseOptions()
	{
		var options = (CSharpParseOptions)base.CreateParseOptions();

		options = options
			.WithFeatures(_features);

		return options;
	}

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> WithReferenceAssemblies(ReferenceAssemblies referenceAssemblies)
	{
		ReferenceAssemblies = referenceAssemblies;
		return this;
	}

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddSource([StringSyntax(LanguageNames.CSharp)] string source)
	{
		TestState.Sources.Add(source);
		return this;
	}

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddExpectedDiagnostics(DiagnosticResult diagnosticResult)
	{
		TestState.ExpectedDiagnostics.Add(diagnosticResult);
		return this;
	}

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddFeature(string key, string value)
	{
		_features ??= [];
		_features.Add(new KeyValuePair<string, string>(key, value));
		return this;
	}

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddAnalyzerReference(ISourceGenerator generator)
	{
		return AddAnalyzerReference(new AdhocAnalyzerReference(generator));
	}

#if !ROSLYN3_11
	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddAnalyzerReference(IIncrementalGenerator generator)
	{
		return AddAnalyzerReference(new AdhocAnalyzerReference(generator));
	}
#endif

	public CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> AddAnalyzerReference(AnalyzerReference analyzerReference)
	{
		SolutionTransforms.Add(Solution (Solution solution, ProjectId projectId) =>
		{
			return solution.AddAnalyzerReference(projectId, analyzerReference);
		});
		return this;
	}
}
