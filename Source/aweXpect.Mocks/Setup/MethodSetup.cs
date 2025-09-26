using System.Threading;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

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
}
