﻿namespace aweXpect.Mocks.Tests;

public sealed class WithTests
{
	[Theory]
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData("foo", true)]
	[InlineData("fo", false)]
	public async Task ImplicitConversion_ShouldCheckForEquality(string? value, bool expectMatch)
	{
		With.MatchParameter<string> sut = "foo";

		bool result = sut.Matches(value);

		await That(result).IsEqualTo(expectMatch);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("foo")]
	public async Task WithAny_ShouldAlwaysMatch(string? value)
	{
		With.MatchParameter<string> sut = With.Any<string>();

		bool result = sut.Matches(value);

		await That(result).IsTrue();
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData(1, false)]
	public async Task WithMatching_CheckForNull_ShouldReturnExpectedResult(int? value, bool expectedResult)
	{
		With.MatchParameter<int?> sut = With<int?>.Matching(v => v is null);

		bool result = sut.Matches(value);

		await That(result).IsEqualTo(expectedResult);
	}

	[Theory]
	[InlineData(42L)]
	[InlineData("foo")]
	public async Task WithMatching_DifferentType_ShouldReturnFalse(object? value)
	{
		With.MatchParameter<int?> sut = With<int?>.Matching(_ => true);

		bool result = sut.Matches(value);

		await That(result).IsFalse();
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task WithMatching_ShouldReturnPredicateValue(bool predicateValue)
	{
		With.MatchParameter<string> sut = With<string>.Matching(_ => predicateValue);

		bool result = sut.Matches("foo");

		await That(result).IsEqualTo(predicateValue);
	}
}
