using aweXpect.Mocks.Internals;
using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.Entities;

internal readonly record struct MockClass
{
	public MockClass(ITypeSymbol[] types)
	{
		Namespace = types[0].ContainingNamespace.ToString();
		ClassName = types[0].Name;

		FileName = $"MockFor{ClassName}.g.cs";
		Methods = new EquatableArray<Method>(
			types[0].GetMembers().OfType<IMethodSymbol>().Select(x => new Method(x)).ToArray());
	}

	public EquatableArray<Method> Methods { get; }
	public string FileName { get; }
	public string Namespace { get; }
	public string ClassName { get; }

	public IEnumerable<string> GetNamespaces()
	{
		yield return Namespace;
		foreach (Method method in Methods)
		{
			if (method.ReturnType.Namespace != null)
			{
				yield return method.ReturnType.Namespace;
			}

			foreach (MethodParameter parameter in method.Parameters)
			{
				if (parameter.Type.Namespace != null)
				{
					yield return parameter.Type.Namespace;
				}
			}
		}
	}
}
