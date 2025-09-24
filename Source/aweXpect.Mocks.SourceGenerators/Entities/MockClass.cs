using aweXpect.Mocks.SourceGenerators.Internals;
using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.SourceGenerators.Entities;

internal record MockClass
{
	private string[]? _namespaces;

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


	public string[] GetNamespaces()
	{
		_namespaces ??= EnumerateNamespaces().Distinct().OrderBy(n => n).ToArray();
		return _namespaces;
	}

	private IEnumerable<string> EnumerateNamespaces()
	{
		yield return Namespace;
		foreach (Method method in Methods)
		{
			if (method.ReturnType.Namespace != null)
			{
				yield return method.ReturnType.Namespace;
			}

			foreach (string? @namespace in method.Parameters
				         .Select(parameter => parameter.Type.Namespace)
				         .Where(n => n is not null))
			{
				yield return @namespace!;
			}
		}
	}
}
