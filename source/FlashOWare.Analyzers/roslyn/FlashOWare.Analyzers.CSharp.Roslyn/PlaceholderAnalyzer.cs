using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FlashOWare.Analyzers.CSharp.Roslyn;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PlaceholderAnalyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor s_rule = new DiagnosticDescriptor(
		"FLASHOWARE0000",
		"Diagnostic Title",
		"Diagnostic Message Format",
		"Category",
		DiagnosticSeverity.Warning,
		true,
		"Diagnostic Description.",
		"https://github.com/FlashOWare/roslyn-components-compiler-extensions",
		WellKnownDiagnosticTags.CompilationEnd
	);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterCompilationAction(OnCompilation);
	}

	private static void OnCompilation(CompilationAnalysisContext context)
	{
		if (context.Options.AnalyzerConfigOptionsProvider.GlobalOptions.TryGetValue("build_property.EnableTrimAnalyzer", out string? trim) &&
			context.Options.AnalyzerConfigOptionsProvider.GlobalOptions.TryGetValue("build_property.EnableAotAnalyzer", out string? aot))
		{
			if (trim.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase) &&
				aot.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}

		var diagnostic = Diagnostic.Create(s_rule, Location.None);
		context.ReportDiagnostic(diagnostic);
	}
}
