using System;
using System.Collections.Generic;
using System.Text;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Tests;

public sealed class WithTests
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("foo")]
	public async Task WithAny_ShouldAlwaysMatch(string? value)
	{
		var sut = With.Any<string>();

		var result = sut.Matches(value);

		await That(result).IsTrue();
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData(1, false)]
	public async Task WithMatching_CheckForNull_ShouldReturnExpectedResult(int? value, bool expectedResult)
	{
		var sut = With<int?>.Matching(v => v is null);

		var result = sut.Matches(value);

		await That(result).IsEqualTo(expectedResult);
	}

	[Theory]
	[InlineData(42L)]
	[InlineData("foo")]
	public async Task WithMatching_DifferentType_ShouldReturnFalse(object? value)
	{
		var sut = With<int?>.Matching(_ => true);

		var result = sut.Matches(value);

		await That(result).IsFalse();
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task WithMatching_ShouldReturnPredicateValue(bool predicateValue)
	{
		var sut = With<string>.Matching(_ => predicateValue);

		var result = sut.Matches("foo");

		await That(result).IsEqualTo(predicateValue);
	}

	[Theory]
	[InlineData(null, false)]
	[InlineData("", false)]
	[InlineData("foo", true)]
	[InlineData("fo", false)]
	public async Task ImplicitConversion_ShouldCheckForEquality(string? value, bool expectMatch)
	{
		With.MatchParameter<string> sut = "foo";

		var result = sut.Matches(value);

		await That(result).IsEqualTo(expectMatch);
	}
}
