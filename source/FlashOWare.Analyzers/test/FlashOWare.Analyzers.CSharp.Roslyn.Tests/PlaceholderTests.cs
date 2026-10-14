namespace FlashOWare.Analyzers.CSharp.Roslyn.Tests;

[TestClass]
public class PlaceholderTests
{
	[TestMethod]
	public void PlaceholderTest()
	{
		Assert.AreEqual("240", new Placeholder(0xF0).ToString());
	}
}
