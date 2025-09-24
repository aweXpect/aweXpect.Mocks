using System.Text;
using aweXpect.Mocks.SourceGenerators.Internals;
using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.SourceGenerators.Entities;

internal readonly record struct Method
{
	public Method(IMethodSymbol methodSymbol)
	{
		Accessibility = methodSymbol.DeclaredAccessibility;
		IsVirtual = methodSymbol.IsVirtual;
		ReturnType = methodSymbol.ReturnsVoid ? Type.Void : new Type(methodSymbol.ReturnType);
		Name = methodSymbol.Name;
		Parameters = new EquatableArray<MethodParameter>(
			methodSymbol.Parameters.Select(x => new MethodParameter(x)).ToArray());
	}

	public bool IsVirtual { get; }

	public Accessibility Accessibility { get; }
	public Type ReturnType { get; }
	public string Name { get; }
	public EquatableArray<MethodParameter> Parameters { get; }

	public void AppendSignatureTo(StringBuilder sb, MockClass @class, string[] namespaces)
	{
		sb.Append(Accessibility.ToVisibilityString()).Append(' ');
		if (!@class.IsInterface && IsVirtual)
		{
			sb.Append("override ");
		}
		sb.Append(ReturnType.GetMinimizedString(namespaces)).Append(' ')
			.Append(Name).Append('(');
		int index = 0;
		foreach (MethodParameter parameter in Parameters)
		{
			if (index++ > 0)
			{
				sb.Append(", ");
			}

			sb.Append(parameter.Type.GetMinimizedString(namespaces)).Append(' ').Append(parameter.Name);
		}

		sb.Append(')');
	}
}
