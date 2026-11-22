using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;

using static Microsoft.CodeAnalysis.Testing.DiagnosticResult;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests.Testing;

public abstract class CSharpDiagnosticAnalyzerFixture<TDiagnosticAnalyzer>
	where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
{
	protected CSharpDiagnosticAnalyzerFixture()
	{
		TestContext = null!;
	}

	protected CSharpDiagnosticAnalyzerFixture(TestContext context)
	{
		TestContext = context;
	}

	public TestContext TestContext { get; set; }

	internal virtual CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer> CreateTest()
	{
		return new CSharpDiagnosticAnalyzerTest<TDiagnosticAnalyzer>()
		{
			CompilerDiagnostics = CompilerDiagnostics.Warnings,
			DisabledDiagnostics =
			{
				"CS1591", // warning CS1591: Missing XML comment for publicly visible type or member 'Type_or_Member'
			},
		};
	}

	internal static DiagnosticResult CreateError(string identifier)
	{
		return CompilerError(identifier);
	}

	internal static DiagnosticResult CreateWarning(string identifier)
	{
		return CompilerWarning(identifier);
	}

	internal static DiagnosticResult CreateDiagnostic(string id, DiagnosticSeverity severity, string message, int markupKey)
	{
		return CSharpAnalyzerVerifier<TDiagnosticAnalyzer, DefaultVerifier>.Diagnostic(id)
			.WithSeverity(severity)
			.WithMessage(message)
			.WithLocation(markupKey);
	}

	[TestMethod]
	public void VerifyType()
	{
		Type type = typeof(TDiagnosticAnalyzer);
		var attribute = type.GetCustomAttribute<DiagnosticAnalyzerAttribute>();

		Assert.IsNotNull(attribute, $"But '{nameof(DiagnosticAnalyzerAttribute)}' not found.");
		Assert.AreSequenceEqual([LanguageNames.CSharp], attribute.Languages, $"Expected language '{LanguageNames.CSharp}' to be supported.");
	}
}
