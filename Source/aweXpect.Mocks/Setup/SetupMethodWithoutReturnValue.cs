using System;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Setup for a method returning <see langword="void" />
/// </summary>
public class SetupMethodWithoutReturnValue(string name) : MockSetup
{
	private Action? _callback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithoutReturnValue Callback(Action callback)
	{
		_callback = callback;
		return this;
	}

	/// <inheritdoc cref="MockSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation) => _callback?.Invoke();

	/// <inheritdoc cref="MockSetup.GetReturnValue{TResult}(Invocation)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
		=> throw new NotSupportedException("The setup does not support return values");

	/// <inheritdoc cref="MockSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 0;
}

/// <summary>
///     Setup for a method with one parameter <typeparamref name="T" /> returning <see langword="void" />.
/// </summary>
public class SetupMethodWithoutReturnValue<T>(string name, MatchParameter match) : MockSetup
{
	private Action<T>? _callback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithoutReturnValue<T> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithoutReturnValue<T> Callback(Action<T> callback)
	{
		_callback = callback;
		return this;
	}

	/// <inheritdoc cref="MockSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation)
	{
		if (invocation.Parameters[0] is T p1)
		{
			_callback?.Invoke(p1);
		}
	}

	/// <inheritdoc cref="MockSetup.GetReturnValue{TResult}(Invocation)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
		=> throw new NotSupportedException("The setup does not support return values");

	/// <inheritdoc cref="MockSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 1 && match.Matches(invocation.Parameters[0]);
}
