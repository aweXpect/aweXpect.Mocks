using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.Entities;

internal readonly record struct Type
{
	private Type(string name)
	{
		Name = name;
	}

	internal Type(ITypeSymbol typeSymbol)
	{
		Name = typeSymbol.ToDisplayString();
		Namespace = typeSymbol.ContainingNamespace.ToString();
	}

	public string? Namespace { get; }

	internal static Type Void { get; } = new("void");

	public string Name { get; }

	public override string ToString() => Name;
}
