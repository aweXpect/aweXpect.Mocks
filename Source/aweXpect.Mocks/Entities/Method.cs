using System.Text;
using aweXpect.Mocks.Internals;
using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.Entities;

internal readonly record struct Method
{
	public Method(IMethodSymbol methodSymbol)
	{
		Accessibility = methodSymbol.DeclaredAccessibility;
		ReturnType = methodSymbol.ReturnsVoid ? Type.Void : new Type(methodSymbol.ReturnType);
		Name = methodSymbol.Name;
		Parameters = new EquatableArray<MethodParameter>(
			methodSymbol.Parameters.Select(x => new MethodParameter(x)).ToArray());
	}

	public Accessibility Accessibility { get; }
	public Type ReturnType { get; }
	public string Name { get; }
	public EquatableArray<MethodParameter> Parameters { get; }

	public void AppendSignatureTo(StringBuilder sb)
	{
		sb.Append(Accessibility.ToVisibilityString()).Append(' ').Append(ReturnType).Append(' ').Append(Name)
			.Append('(');
		int index = 0;
		foreach (MethodParameter parameter in Parameters)
		{
			if (index++ > 0)
			{
				sb.Append(", ");
			}

			sb.Append(parameter.Type).Append(' ').Append(parameter.Name);
		}

		sb.Append(')');
	}
}