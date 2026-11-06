using System.Reflection;
using Microsoft.CodeAnalysis;

namespace FlashOWare.CodeAnalysis.Generators.CSharp.Roslyn.Tests;

[TestClass]
public sealed class PlaceholderTests
{
	[TestMethod]
	public void PlaceholderTest()
	{
		CustomAttributeTypedArgument argument = typeof(PlaceholderGenerator).CustomAttributes
			.Single(static (CustomAttributeData attribute) => attribute.AttributeType.FullName == "Microsoft.CodeAnalysis.GeneratorAttribute")
			.ConstructorArguments.First();
		Assert.AreEqual(LanguageNames.CSharp, argument.Value);
	}
}
