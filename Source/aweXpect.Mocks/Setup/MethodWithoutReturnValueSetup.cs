using System;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Setup for a method returning <see langword="void" />
/// </summary>
public class MethodWithoutReturnValueSetup(string name) : MethodSetup
{
	private Action? _callback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithoutReturnValueSetup Callback(Action callback)
	{
		_callback = callback;
		return this;
	}

	/// <inheritdoc cref="MethodSetup.ExecuteCallback(Invocation)" />
	protected override void ExecuteCallback(Invocation invocation) => _callback?.Invoke();

	/// <inheritdoc cref="MethodSetup.GetReturnValue{TResult}(Invocation)" />
	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
		=> throw new NotSupportedException("The setup does not support return values");

	/// <inheritdoc cref="MethodSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 0;
}

/// <summary>
///     Setup for a method with one parameter <typeparamref name="T" /> returning <see langword="void" />.
/// </summary>
public class MethodWithoutReturnValueSetup<T>(string name, With.MatchParameter match) : MethodSetup
{
	private Action<T>? _callback;

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithoutReturnValueSetup<T> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	/// <summary>
	///     Registers a <paramref name="callback" /> to execute when the method is called.
	/// </summary>
	public MethodWithoutReturnValueSetup<T> Callback(Action<T> callback)
	{
		_callback = callback;
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
		=> throw new NotSupportedException("The setup does not support return values");

	/// <inheritdoc cref="MethodSetup.Matches(Invocation)" />
	public override bool Matches(Invocation invocation)
		=> invocation is MethodInvocation methodInvocation && methodInvocation.Name.Equals(name) &&
		   methodInvocation.Parameters.Length == 1 && match.Matches(methodInvocation.Parameters[0]);
}
