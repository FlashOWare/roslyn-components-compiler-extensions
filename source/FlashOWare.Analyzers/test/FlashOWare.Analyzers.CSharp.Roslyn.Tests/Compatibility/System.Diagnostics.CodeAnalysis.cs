namespace System.Diagnostics.CodeAnalysis;

#if !NET7_0_OR_GREATER
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class StringSyntaxAttribute : Attribute
{
	public StringSyntaxAttribute(string syntax)
	{
		Syntax = syntax;
		Arguments = [];
	}

	public StringSyntaxAttribute(string syntax, params object?[] arguments)
	{
		Syntax = syntax;
		Arguments = arguments;
	}

	public string Syntax { get; }
	public object?[] Arguments { get; }
}
#endif
