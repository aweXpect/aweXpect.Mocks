using System;

namespace aweXpect.Mocks;

/// <summary>
///     Allows the specification of a matching condition for an argument in a method invocation or setup.
/// </summary>
public static class With
{
	/// <summary>
	///     Matches any parameter of type <typeparamref name="T" />.
	/// </summary>
	public static MatchParameter<T> Any<T>() => new MatchParameterType<T>();

	private sealed class MatchParameterType<T> : MatchParameter<T>
	{
		protected override bool Matches(T value) => true;
	}
}

/// <summary>
///     Allows the specification of a matching condition for an argument in a method invocation or setup.
/// </summary>
public static class With<T>
{
	/// <summary>
	///     Matches parameters of type <typeparamref name="T" />, if the <paramref name="predicate" /> returns
	///     <see langword="true" />
	/// </summary>
	public static MatchParameter<T> Matching(Func<T, bool> predicate)
		=> new MatchParameterPredicate(predicate);

	private sealed class MatchParameterPredicate(Func<T, bool> predicate) : MatchParameter<T>
	{
		protected override bool Matches(T value) => predicate(value);
	}
}
