using System;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Setup for a method returning <typeparamref name="TReturn" />.
/// </summary>
public class MethodWithReturnValueSetup<TReturn>(string name) : MethodSetup
{
	private Action? _callback;
	private Func<TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn> Callback(Action callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn> Returns(Func<TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn> Returns(TReturn returnValue)
	{
		_returnCallback = () => returnValue;
		return this;
	}

	/// <inheritdoc cref="MethodSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation) => _callback?.Invoke();

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation)" />
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

	/// <inheritdoc cref="MethodSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 0;
}

/// <summary>
///     Setup for a method with one parameter <typeparamref name="T" /> returning <typeparamref name="TReturn" />.
/// </summary>
public class MethodWithReturnValueSetup<TReturn, T>(string name, With.MatchParameter match)
	: MethodSetup
{
	private Action<T>? _callback;
	private Func<T, TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T> Callback(Action<T> callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T> Returns(Func<T, TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T> Returns(Func<TReturn> callback)
	{
		_returnCallback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T> Returns(TReturn returnValue)
	{
		_returnCallback = _ => returnValue;
		return this;
	}

	/// <inheritdoc cref="MethodSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation)
	{
		if (invocation is MethodInvocation methodInvocation && methodInvocation.Parameters[0] is T p1)
		{
			_callback?.Invoke(p1);
		}
	}

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
	{
		if (_returnCallback is null)
		{
			throw new NotSupportedException("No return value is specified");
		}

		if (invocation is MethodInvocation methodInvocation && methodInvocation.Parameters[0] is T p1 &&
		    _returnCallback(p1) is TResult result)
		{
			return result;
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 1 && match.Matches(methodInvocation.Parameters[0]);
}
