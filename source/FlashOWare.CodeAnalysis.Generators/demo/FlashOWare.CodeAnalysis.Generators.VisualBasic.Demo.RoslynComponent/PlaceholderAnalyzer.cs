using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace FlashOWare.CodeAnalysis.Generators.VisualBasic.Demo.RoslynComponent;

[DiagnosticAnalyzer(LanguageNames.VisualBasic)]
public sealed class PlaceholderAnalyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor s_rule = new DiagnosticDescriptor(
		"F0CADEMOVB0001",
		"Demo Title",
		"Demo Message: {0}",
		"Demo Category",
		DiagnosticSeverity.Info,
		true,
		"Demo Description.",
		"https://github.com/FlashOWare/roslyn-components-compiler-extensions"
	);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [s_rule];

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSyntaxNodeAction(OnSyntaxNodeAnalysis, SyntaxKind.InvocationExpression);
	}

	private static void OnSyntaxNodeAnalysis(SyntaxNodeAnalysisContext context)
	{
		var node = (InvocationExpressionSyntax)context.Node;

		if (node is { Expression: MemberAccessExpressionSyntax { Name: IdentifierNameSyntax identifier } })
		{
			var diagnostic = Diagnostic.Create(s_rule, node.GetLocation(), identifier.Identifier.Text);
			context.ReportDiagnostic(diagnostic);
		}
	}
}
