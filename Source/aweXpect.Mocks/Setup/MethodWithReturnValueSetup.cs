﻿using System;
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

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation, MockBehavior)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation, MockBehavior behavior)
		where TResult : default
	{
		if (_returnCallback is null)
		{
			return behavior.DefaultValueGenerator.Generate<TResult>();
		}

		if (_returnCallback() is TResult result)
		{
			return result;
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.IsMatch(Invocation)" />
	protected override bool IsMatch(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 0;

	/// <inheritdoc cref="MethodSetup.SetOutParameter{T}(string, MockBehavior)" />
	internal protected override T SetOutParameter<T>(string parameterName, MockBehavior behavior)
		=> behavior.DefaultValueGenerator.Generate<T>();

	/// <inheritdoc cref="MethodSetup.SetRefParameter{T}(string, MockBehavior, T)" />
	internal protected override T SetRefParameter<T>(string parameterName, MockBehavior behavior, T value)
		=> behavior.DefaultValueGenerator.Generate<T>();
}

/// <summary>
///     Setup for a method with one parameter <typeparamref name="T1" /> returning <typeparamref name="TReturn" />.
/// </summary>
public class MethodWithReturnValueSetup<TReturn, T1>(string name, With.NamedParameter match1)
	: MethodSetup
{
	private Action<T1>? _callback;
	private Func<T1, TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1> Callback(Action<T1> callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1> Returns(Func<T1, TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1> Returns(Func<TReturn> callback)
	{
		_returnCallback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1> Returns(TReturn returnValue)
	{
		_returnCallback = _ => returnValue;
		return this;
	}

	/// <inheritdoc cref="MethodSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation)
	{
		if (invocation is MethodInvocation methodInvocation)
		{
			if (methodInvocation.Parameters[0].TryCast<T1>(out var p1))
			{
				_callback?.Invoke(p1);
				return;
			}
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation, MockBehavior)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation, MockBehavior behavior)
		where TResult : default
	{
		if (_returnCallback is null)
		{
			return behavior.DefaultValueGenerator.Generate<TResult>();
		}

		if (invocation is MethodInvocation methodInvocation)
		{
			if (methodInvocation.Parameters[0].TryCast<T1>(out var p1) && _returnCallback(p1) is TResult result)
			{
				_callback?.Invoke(p1);
				return result;
			}
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.IsMatch(Invocation)" />
	protected override bool IsMatch(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 1 && match1.Parameter.Matches(methodInvocation.Parameters[0]);

	/// <inheritdoc cref="MethodSetup.SetOutParameter{T}(string, MockBehavior)" />
	internal protected override T SetOutParameter<T>(string parameterName, MockBehavior behavior)
	{
		if (HasOutParameter([match1], parameterName, out With.OutParameter<T>? outParameter))
		{
			return outParameter.GetValue();
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}

	/// <inheritdoc cref="MethodSetup.SetRefParameter{T}(string, MockBehavior, T)" />
	internal protected override T SetRefParameter<T>(string parameterName, MockBehavior behavior, T value)
	{
		if (HasRefParameter([match1], parameterName, out With.RefParameter<T>? refParameter))
		{
			return refParameter.GetValue(value);
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}
}

/// <summary>
///     Setup for a method with two parameters <typeparamref name="T1" /> and <typeparamref name="T2" /> returning <typeparamref name="TReturn" />.
/// </summary>
public class MethodWithReturnValueSetup<TReturn, T1, T2>(string name, With.NamedParameter match1, With.NamedParameter match2)
	: MethodSetup
{
	private Action<T1, T2>? _callback;
	private Func<T1, T2, TReturn>? _returnCallback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1, T2> Callback(Action callback)
	{
		_callback = (_, _) => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1, T2> Callback(Action<T1, T2> callback)
	{
		_callback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1, T2> Returns(Func<T1, T2, TReturn> callback)
	{
		_returnCallback = callback;
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to get the return value for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1, T2> Returns(Func<TReturn> callback)
	{
		_returnCallback = (_, _) => callback();
		return this;
	}

	/// <summary>
	///     Registers the <paramref name="returnValue" /> for this method.
	/// </summary>
	public MethodWithReturnValueSetup<TReturn, T1, T2> Returns(TReturn returnValue)
	{
		_returnCallback = (_, _) => returnValue;
		return this;
	}

	/// <inheritdoc cref="MethodSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation)
	{
		if (invocation is MethodInvocation methodInvocation)
		{
			if (methodInvocation.Parameters[0].TryCast<T1>(out var p1) &&
				methodInvocation.Parameters[1].TryCast<T2>(out var p2))
			{
				_callback?.Invoke(p1, p2);
				return;
			}
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation, MockBehavior)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation, MockBehavior behavior)
		where TResult : default
	{
		if (_returnCallback is null)
		{
			return behavior.DefaultValueGenerator.Generate<TResult>();
		}

		if (invocation is MethodInvocation methodInvocation)
		{
			if (methodInvocation.Parameters[0].TryCast<T1>(out var p1) &&
				methodInvocation.Parameters[1].TryCast<T2>(out var p2) &&
				_returnCallback(p1, p2) is TResult result)
			{
				_callback?.Invoke(p1, p2);
				return result;
			}
		}

		throw new NotSupportedException("The method type does not match");
	}

	/// <inheritdoc cref="MethodSetup.IsMatch(Invocation)" />
	protected override bool IsMatch(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   Matches([match1, match2], methodInvocation.Parameters);

	/// <inheritdoc cref="MethodSetup.SetOutParameter{T}(string, MockBehavior)" />
	internal protected override T SetOutParameter<T>(string parameterName, MockBehavior behavior)
	{
		if (HasOutParameter([match1, match2], parameterName, out With.OutParameter<T>? outParameter))
		{
			return outParameter.GetValue();
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}

	/// <inheritdoc cref="MethodSetup.SetRefParameter{T}(string, MockBehavior, T)" />
	internal protected override T SetRefParameter<T>(string parameterName, MockBehavior behavior, T value)
	{
		if (HasRefParameter([match1, match2], parameterName, out With.RefParameter<T>? refParameter))
		{
			return refParameter.GetValue(value);
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}
}
