using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using aweXpect.Mocks.SourceGenerators.Entities;
using aweXpect.Mocks.SourceGenerators.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace aweXpect.Mocks.SourceGenerators;

/// <summary>
///     The <see cref="IIncrementalGenerator" /> for generating mocks.
/// </summary>
[Generator]
public class MockGenerator : IIncrementalGenerator
{
	void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
	{
		ConcurrentDictionary<string, MockClass> mocks = new();
		IncrementalValuesProvider<MockClass> expectationsToGenerate = context.SyntaxProvider
			.CreateSyntaxProvider(
				static (s, _) => s.IsMockForInvocationExpressionSyntax(),
				(ctx, _) => GetSemanticTargetForGeneration(ctx, mocks))
			.Where(static m => m is not null)
			.SelectMany((x, _) => x!.ToImmutableArray());

		context.RegisterSourceOutput(expectationsToGenerate, (spc, source) => Execute(source, spc));
	}

	private static IEnumerable<MockClass> GetSemanticTargetForGeneration(GeneratorSyntaxContext context,
		ConcurrentDictionary<string, MockClass> files)
	{
		if (context.Node.TryExtractGenericNameSyntax(context.SemanticModel, out GenericNameSyntax? genericNameSyntax))
		{
			SemanticModel semanticModel = context.SemanticModel;

			ITypeSymbol[] types = genericNameSyntax.TypeArgumentList.Arguments
				.Select(t => semanticModel.GetTypeInfo(t).Type)
				.Where(t => t is not null)
				.Cast<ITypeSymbol>()
				.ToArray();
			MockClass mockClassClass = new(types);
			if (files.TryAdd(mockClassClass.FileName, mockClassClass))
			{
				yield return mockClassClass;
			}
		}
	}

	private static void Execute(MockClass mockClass, SourceProductionContext context)
	{
		string result = SourceGeneration.GetMockClass(mockClass);
		// Create a separate class file for each mock
		context.AddSource(mockClass.FileName, SourceText.From(result, Encoding.UTF8));
	}
}
