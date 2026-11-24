using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests;

[TestClass]
public sealed class CompilerExtensionsTests
{
	[TestMethod]
	public void CompilerExtensions()
	{
#if ROSLYN3_11
		IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(static (Assembly assembly) => assembly.FullName!.StartsWith("FlashOWare.Analyzers.CSharp.Roslyn", StringComparison.Ordinal));
		Assert.ContainsSingle(assemblies, "Delete this preprocessor directive when the Roslyn 3.11 Component also contains a Compiler Extension.");
		return;
#endif

		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Single(static (Assembly assembly) =>
		{
			Type type = typeof(CompilerExtensionsTests);
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			string name = type.Assembly.GetName().Name!.Replace(".Tests", "", StringComparison.Ordinal);
#else
			string name = type.Assembly.GetName().Name.Replace(".Tests", "");
#endif
			return assembly.FullName!.StartsWith($"{name}, Version=", StringComparison.Ordinal);
		});

		Type?[] types =
		[
#if ROSLYN_LATEST || ROSLYN4_13 || ROSLYN3_11
#endif

#if ROSLYN_LATEST || ROSLYN4_13
			assembly.GetType("FlashOWare.Analyzers.CSharp.Roslyn.InterceptorMethodDiagnosticAnalyzer"),
#endif
		];

		IEnumerable<Type> extensions = assembly.ExportedTypes.Where(static (Type type) => type.BaseType == typeof(DiagnosticAnalyzer));

		Assert.AreAllNotNull(types);
		Assert.AreSequenceEqual(types, extensions, SequenceOrder.InAnyOrder);
	}
}
