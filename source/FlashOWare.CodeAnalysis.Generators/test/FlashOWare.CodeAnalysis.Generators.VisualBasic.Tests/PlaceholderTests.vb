Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass>
Public NotInheritable Class PlaceholderTests

	<TestMethod>
	Public Sub PlaceholderTest()
		Assert.AreEqual("240", New Generated.Placeholder(&HF0).ToString())
	End Sub

End Class
