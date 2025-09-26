using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

public class MethodSetupResult(MethodSetup? setup, MockBehavior behavior)
{
	public T SetOutParameter<T>(string parameterName)
	{
		if (setup is not null)
		{
			return setup.SetOutParameter<T>(parameterName, behavior);
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}
	public T SetRefParameter<T>(string parameterName, T value)
	{
		if (setup is not null)
		{
			return setup.SetRefParameter<T>(parameterName, behavior, value);
		}

		return behavior.DefaultValueGenerator.Generate<T>();
	}
}
public class MethodSetupResult<TResult>(MethodSetup? setup, MockBehavior behavior, TResult result) : MethodSetupResult(setup, behavior)
{
	public TResult Result { get; } = result;
}

/// <summary>
///     Base class for method setups.
/// </summary>
public abstract class MethodSetup : IMethodSetup
{
	private int _invocationCount;

	/// <inheritdoc cref="IMethodSetup.InvocationCount" />
	int IMethodSetup.InvocationCount => _invocationCount;

	internal TResult Invoke<TResult>(Invocation invocation, MockBehavior behavior)
	{
		Interlocked.Increment(ref _invocationCount);
		ExecuteCallback(invocation);
		return GetReturnValue<TResult>(invocation, behavior);
	}

	internal void Invoke(Invocation invocation)
	{
		Interlocked.Increment(ref _invocationCount);
		ExecuteCallback(invocation);
	}

	internal protected abstract T SetOutParameter<T>(string parameterName, MockBehavior behavior);

	internal protected abstract T SetRefParameter<T>(string parameterName, MockBehavior behavior, T value);

	/// <summary>
	///     Execute a potentially registered callback.
	/// </summary>
	protected abstract void ExecuteCallback(Invocation invocation);

	/// <summary>
	///     Gets the registered return value.
	/// </summary>
	protected abstract TResult GetReturnValue<TResult>(Invocation invocation, MockBehavior behavior);

	/// <inheritdoc cref="IMethodSetup.Matches(Invocation)" />
	bool IMethodSetup.Matches(Invocation invocation)
		=> IsMatch(invocation);

	/// <summary>
	///     Checks if the <paramref name="invocation" /> matches the setup.
	/// </summary>
	protected abstract bool IsMatch(Invocation invocation);

	internal static bool HasRefParameter<T>(With.NamedParameter[] namedParameters, string parameterName, [NotNullWhen(true)] out With.RefParameter<T>? parameter)
	{
		foreach (var namedParameter in namedParameters)
		{
			if (namedParameter.Name.Equals(parameterName, StringComparison.Ordinal) &&
				namedParameter.Parameter is With.RefParameter<T> refParameter)
			{
				parameter = refParameter;
				return true;
			}
		}
		parameter = null;
		return false;
	}

	internal static bool HasOutParameter<T>(With.NamedParameter[] namedParameters, string parameterName, [NotNullWhen(true)] out With.OutParameter<T>? parameter)
	{
		foreach (var namedParameter in namedParameters)
		{
			if (namedParameter.Name.Equals(parameterName, StringComparison.Ordinal) &&
				namedParameter.Parameter is With.OutParameter<T> outParameter)
			{
				parameter = outParameter;
				return true;
			}
		}
		parameter = null;
		return false;
	}

	internal static bool Matches(With.NamedParameter[] namedParameters, object?[] values)
	{
		if (namedParameters.Length != values.Length)
		{
			return false;
		}
		for (int i = 0; i < namedParameters.Length; i++)
		{
			if (!namedParameters[i].Parameter.Matches(values[i]))
			{
				return false;
			}
		}
		return true;
	}
}
