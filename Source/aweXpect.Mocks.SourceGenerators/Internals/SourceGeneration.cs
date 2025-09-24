using Microsoft.CodeAnalysis;

namespace aweXpect.Mocks.SourceGenerators.Internals;

internal static partial class SourceGeneration
{
	public const string Mock =
		"""
		using System;

		namespace aweXpect.Mocks;

		#nullable enable
		public static partial class Mock
		{
			public static Mock<T> For<T>()
			{
				var generator = new MockGenerator();
				return generator.Get<T>()
					?? throw new NotSupportedException("Could not generate Mock<T>");
			}
			
			private partial class MockGenerator
			{
				private object? _value;
				partial void Generate<T>();
				public Mock<T>? Get<T>()
				{
					Generate<T>();
					return _value as Mock<T>;
				}
			}
		}
		#nullable disable
		""";

	internal static string ToVisibilityString(this Accessibility accessibility)
		=> accessibility switch
		{
			Accessibility.Private => "private",
			Accessibility.Protected => "protected",
			Accessibility.Internal => "internal",
			Accessibility.ProtectedOrInternal => "protected internal",
			Accessibility.Public => "public",
			Accessibility.ProtectedAndInternal => "protected internal",
			_ => throw new ArgumentOutOfRangeException(nameof(accessibility), accessibility, null)
		};
}
