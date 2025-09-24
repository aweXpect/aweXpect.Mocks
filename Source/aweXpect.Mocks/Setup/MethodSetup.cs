using System.Threading;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Base class for method setups.
/// </summary>
public abstract class MethodSetup
{
	private int _invocationCount;

	/// <summary>
	///     The number of matching invocations on the mock.
	/// </summary>
	public int InvocationCount => _invocationCount;

	internal TResult Invoke<TResult>(Invocation invocation)
	{
		Interlocked.Increment(ref _invocationCount);
		ExecuteCallback(invocation);
		return GetReturnValue<TResult>(invocation);
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
	protected abstract TResult GetReturnValue<TResult>(Invocation invocation);

	/// <summary>
	///     Checks if the <paramref name="invocation" /> matches the setup.
	/// </summary>
	public abstract bool Matches(Invocation invocation);
}
