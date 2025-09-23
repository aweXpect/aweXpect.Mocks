namespace aweXpect.Mocks;

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
	public override bool Matches(object? value) => value is T typedValue && Matches(typedValue);

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

		protected override bool Matches(T value) => value?.Equals(_value) == true;
	}
}
