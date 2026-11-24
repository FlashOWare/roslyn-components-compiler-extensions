using Microsoft.CodeAnalysis.Diagnostics;

namespace FlashOWare.Analyzers.CSharp.Roslyn;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class InterceptorMethodDiagnosticAnalyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor s_rule = new(
		DiagnosticIds.Analyzers.FLASHOWARE0001,
		"Intercepted method invocation",
		"Method invocation '{0}' is intercepted by method '{1}'",
		Categories.Roslyn,
		DiagnosticSeverity.Info,
		true,
		"An interceptor is a method that substitutes a call to an interceptable method with a call to itself at compile time, typically created via a source generator.",
		"https://github.com/FlashOWare/roslyn-components-compiler-extensions",
		Array.Empty<string>()
	);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSyntaxNodeAction(OnSyntaxNode, ImmutableArray.Create(SyntaxKind.InvocationExpression));
	}

	private static void OnSyntaxNode(SyntaxNodeAnalysisContext context)
	{
		var node = (InvocationExpressionSyntax)context.Node;

		IMethodSymbol? symbol = context.SemanticModel.GetInterceptorMethod(node, context.CancellationToken);

		if (symbol is null)
		{
			return;
		}

		SymbolInfo info = context.SemanticModel.GetSymbolInfo(node, context.CancellationToken);

		Location location = node.Expression.GetLocation();
		var diagnostic = Diagnostic.Create(s_rule, location, [info.Symbol?.Name, symbol.Name]);
		context.ReportDiagnostic(diagnostic);
	}
}
