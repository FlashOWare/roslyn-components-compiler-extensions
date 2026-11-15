using System.Reflection;
using Microsoft.CodeAnalysis;

namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests;

[TestClass]
public sealed class PlaceholderTests
{
	[TestMethod]
	public void PlaceholderTest()
	{
		CustomAttributeTypedArgument argument = typeof(PlaceholderAnalyzer).CustomAttributes
			.Single(static (CustomAttributeData attribute) => attribute.AttributeType.FullName == "Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzerAttribute")
			.ConstructorArguments.First();
		Assert.AreEqual(LanguageNames.CSharp, argument.Value);
	}
}
