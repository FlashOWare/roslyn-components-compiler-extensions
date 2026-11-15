Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public NotInheritable Class PlaceholderAnalyzer
	Inherits DiagnosticAnalyzer

	Private Shared ReadOnly s_rule As New DiagnosticDescriptor(
		"F0CADEMOVB0001",
		"Demo Title",
		"Demo Message: {0}",
		"Demo Category",
		DiagnosticSeverity.Info,
		True,
		"Demo Description.",
		"https://github.com/FlashOWare/roslyn-components-compiler-extensions",
		Array.Empty(Of String)
	)

	Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
		Get
			Return ImmutableArray.Create(s_rule)
		End Get
	End Property

	Public Overrides Sub Initialize(context As AnalysisContext)
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None)
		context.EnableConcurrentExecution()

		context.RegisterSyntaxNodeAction(AddressOf OnSyntaxNodeAnalysis, SyntaxKind.InvocationExpression)
	End Sub

	Private Shared Sub OnSyntaxNodeAnalysis(context As SyntaxNodeAnalysisContext)
		Dim node = DirectCast(context.Node, InvocationExpressionSyntax)

		Dim memberAccess = TryCast(node.Expression, MemberAccessExpressionSyntax)
		If memberAccess Is Nothing Then
			Return
		End If

		Dim identifier = TryCast(memberAccess.Name, IdentifierNameSyntax)
		If identifier Is Nothing Then
			Return
		End If

		Dim diag = Diagnostic.Create(s_rule, node.GetLocation(), identifier.Identifier.Text)
		context.ReportDiagnostic(diag)
	End Sub
End Class
