using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using aweXpect.Mocks.Entities;
using aweXpect.Mocks.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace aweXpect.Mocks;

/// <summary>
///     The <see cref="IIncrementalGenerator" /> for the registration of mocks.
/// </summary>
[Generator]
public class MockRegistrationGenerator : IIncrementalGenerator
{
	void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
	{
		ConcurrentDictionary<string, MockClass> files = new();
		IncrementalValueProvider<ImmutableArray<MockClass?>> expectationsToRegister = context.SyntaxProvider
			.CreateSyntaxProvider(
				static (s, _) => s.IsMockForInvocationExpressionSyntax(),
				(ctx, _) => GetSemanticTargetForGeneration(ctx, files))
			.Where(static m => m is not null)
			.Collect();

		context.RegisterSourceOutput(expectationsToRegister,
			(spc, source) => Execute([..source.Where(t => t != null).Cast<MockClass>(),], spc));
	}

	private static MockClass? GetSemanticTargetForGeneration(GeneratorSyntaxContext context,
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
				return mockClassClass;
			}
		}

		return null;
	}

	private static void Execute(ImmutableArray<MockClass> mocksToGenerate, SourceProductionContext context)
		=> context.AddSource("Mock.Registration.g.cs",
			SourceText.From(SourceGeneration.RegisterMocks(mocksToGenerate), Encoding.UTF8));
}
