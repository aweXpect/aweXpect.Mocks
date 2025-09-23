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
	public static T Any<T>()
	{
		return default!;
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
	public static T Matching(Func<T, bool> predicate)
	{
		return default!;
	}
	/// <summary>
	///     Matches parameters of type <typeparamref name="T" />, if it is equal to the expected <paramref name="value"/>.
	/// </summary>
	public static T EqualTo(T value)
	{
		return default!;
	}
}
