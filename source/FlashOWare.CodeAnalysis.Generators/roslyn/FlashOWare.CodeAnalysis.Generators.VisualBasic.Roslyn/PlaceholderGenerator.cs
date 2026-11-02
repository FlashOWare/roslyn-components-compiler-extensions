using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FlashOWare.CodeAnalysis.Generators.VisualBasic.Roslyn;

[Generator(LanguageNames.VisualBasic)]
public sealed class PlaceholderGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(static (IncrementalGeneratorPostInitializationContext context) =>
		{
			string source = """
				Namespace Generated
					Public NotInheritable Class Placeholder
						Private ReadOnly number As Integer

						Public Sub New(ByVal number As Integer)
							Me.number = number
						End Sub

						Public Overrides Function ToString() As String
							Return $"{number}"
						End Function
					End Class
				End Namespace
				""";
			context.AddSource("Placeholder.g.vb", SourceText.From(source, Encoding.UTF8));
		});
	}
}
