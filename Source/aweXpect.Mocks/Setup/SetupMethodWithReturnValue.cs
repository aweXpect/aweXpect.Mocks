using System;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Setup for a method returning <typeparamref name="TReturn" />.
/// </summary>
public class SetupMethodWithReturnValue<TReturn>(string name) : MockSetup
{
	private Action? _callback;
	private Func<TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn> Callback(Action callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn> Returns(Func<TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn> Returns(TReturn returnValue)
	{
		_returnCallback = () => returnValue;
		return this;
	}

	/// <inheritdoc cref="MockSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation) => _callback?.Invoke();

	/// <inheritdoc cref="MockSetup.GetReturnValue{TResult}(Invocation)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
	{
		if (_returnCallback is null)
		{
			throw new NotSupportedException("No return value is specified");
		}

		if (_returnCallback() is TResult result)
		{
			return result;
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MockSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 0;
}

/// <summary>
///     Setup for a method with one parameter <typeparamref name="T" /> returning <typeparamref name="TReturn" />.
/// </summary>
public class SetupMethodWithReturnValue<TReturn, T>(string name, MatchParameter match)
	: MockSetup
{
	private Action<T>? _callback;
	private Func<T, TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn, T> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn, T> Callback(Action<T> callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn, T> Returns(Func<T, TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn, T> Returns(Func<TReturn> callback)
	{
		_returnCallback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public SetupMethodWithReturnValue<TReturn, T> Returns(TReturn returnValue)
	{
		_returnCallback = _ => returnValue;
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
	{
		if (_returnCallback is null)
		{
			throw new NotSupportedException("No return value is specified");
		}

		if (invocation.Parameters[0] is T p1 && _returnCallback(p1) is TResult result)
		{
			return result;
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MockSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 1 && match.Matches(invocation.Parameters[0]);
}
