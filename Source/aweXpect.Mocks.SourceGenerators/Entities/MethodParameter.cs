using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.SourceGenerators.Entities;

internal readonly record struct MethodParameter
{
	public MethodParameter(IParameterSymbol parameterSymbol)
	{
		Type = new Type(parameterSymbol.Type);
		Name = parameterSymbol.Name;
	}

	public Type Type { get; }
	public string Name { get; }
}
