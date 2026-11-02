namespace FlashOWare.CodeAnalysis.Generators.CSharp.Tests;

[TestClass]
public sealed class PlaceholderTests
{
	[TestMethod]
	public void PlaceholderTest()
	{
		Assert.AreEqual("240", new Generated.Placeholder(0xF0).ToString());
	}
}
