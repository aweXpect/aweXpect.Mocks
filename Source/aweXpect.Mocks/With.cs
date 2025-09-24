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

	/// <summary>
	///     Matches a method parameter against an expectation.
	/// </summary>
	public abstract class MatchParameter
	{
		/// <summary>
		///     <see langword="true" />, if the <paramref name="value" /> matches the expectation;
		///     otherwise <see langword="false" />
		/// </summary>
		public abstract bool Matches(object? value);
	}

	/// <summary>
	///     Matches a method parameter against an expectation.
	/// </summary>
	public abstract class MatchParameter<T> : MatchParameter
	{
		/// <summary>
		///     <see langword="true" />, if the <paramref name="value" /> is of type <typeparamref name="T" /> and
		///     matches the expectation; otherwise <see langword="false" />
		/// </summary>
		public override bool Matches(object? value)
		{
			if (value is T typedValue)
			{
				return Matches(typedValue);
			}

			return value is null && Matches(default!);
		}

		/// <summary>
		///     Verifies the expectation.
		/// </summary>
		protected abstract bool Matches(T value);

		/// <summary>
		///     Implicitly converts to a <see cref="MatchParameter{T}" /> the compares the <paramref name="value" /> for equality.
		/// </summary>
		public static implicit operator MatchParameter<T>(T value)
		{
			return new MatchParameterIsEqual(value);
		}

		private sealed class MatchParameterIsEqual : MatchParameter<T>
		{
			private readonly T _value;

			public MatchParameterIsEqual(T value)
			{
				_value = value;
			}

			protected override bool Matches(T value) => Equals(value, _value);
		}
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
	public static With.MatchParameter<T> Matching(Func<T, bool> predicate)
		=> new MatchParameterPredicate(predicate);

	private sealed class MatchParameterPredicate(Func<T, bool> predicate) : With.MatchParameter<T>
	{
		protected override bool Matches(T value) => predicate(value);
	}
}
