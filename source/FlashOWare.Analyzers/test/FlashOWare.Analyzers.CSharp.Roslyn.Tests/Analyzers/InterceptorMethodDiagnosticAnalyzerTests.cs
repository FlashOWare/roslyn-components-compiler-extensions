using System.Globalization;
using FlashOWare.Analyzers.CSharp.Roslyn.Tests.Generators;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests;

[TestClass]
public sealed class InterceptorMethodDiagnosticAnalyzerTests : CSharpDiagnosticAnalyzerFixture<InterceptorMethodDiagnosticAnalyzer>
{
	[TestMethod]
	public async Task NoInterceptor_NoDiagnostic()
	{
		await CreateTest()
			.AddSource("""
				using System;

				public static class Class
				{
					public static void Method()
					{
						Console.WriteLine("Hello, World!");
						Console.Out.WriteLine("Hello, World!");
						Console.Error.WriteLine("Hello, World!");
					}
				}
				""")
			.RunAsync(TestContext.CancellationToken);
	}

	[TestMethod]
	public async Task WithInterceptor_ReportDiagnostic()
	{
		await CreateTest()
			.AddSource("""
				using System;

				public static class Class
				{
					public static void Method()
					{
						{|#0:Console.WriteLine|#0}("Hello, World!");
						{|#1:Console.Out.WriteLine|#1}("Hello, World!");
						{|#2:Console.Error.WriteLine|#2}("Hello, World!");
					}
				}
				""")
			.AddExpectedDiagnostics(CreateDiagnostic(0, "WriteLine", "Interceptor0"))
			.AddExpectedDiagnostics(CreateDiagnostic(1, "WriteLine", "Interceptor1"))
			.AddExpectedDiagnostics(CreateDiagnostic(2, "WriteLine", "Interceptor2"))
			.AddFeature("InterceptorsNamespaces", "FlashOWare.Generated")
			.AddAnalyzerReference(new InterceptorMethodGenerator())
			.RunAsync(TestContext.CancellationToken);
	}

	internal override CSharpDiagnosticAnalyzerTest<InterceptorMethodDiagnosticAnalyzer> CreateTest()
	{
		return base.CreateTest()
			.WithReferenceAssemblies(ReferenceAssemblies.Net.Net90);
	}

	private static DiagnosticResult CreateDiagnostic(int markupKey, params object[] arguments)
	{
		string message = String.Format(CultureInfo.InvariantCulture, "Method invocation '{0}' is intercepted by method '{1}'", arguments);
		return CreateDiagnostic("FLASHOWARE0001", DiagnosticSeverity.Info, message, markupKey);
	}
}
