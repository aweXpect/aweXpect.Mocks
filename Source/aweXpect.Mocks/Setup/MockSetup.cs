using System.Threading;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Base class for mock setups.
/// </summary>
public abstract class MockSetup
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

/// <summary>
///     Allows registration of <see cref="MockSetup" /> in the mock.
/// </summary>
public class MockSetup<T>(Mock<T> mock) : IMockSetup
{
	/// <inheritdoc cref="IMockSetup.RegisterSetup(MockSetup)" />
	void IMockSetup.RegisterSetup(MockSetup mockSetup)
		=> ((IMockSetup)mock).RegisterSetup(mockSetup);

	/// <inheritdoc cref="IMockSetup.Execute{TResult}(string, object[])" />
	public TResult Execute<TResult>(string name, params object[] args)
		=> ((IMockSetup)mock).Execute<TResult>(name, args);

	/// <inheritdoc cref="IMockSetup.Execute(string, object[])" />
	public void Execute(string name, params object[] args)
		=> ((IMockSetup)mock).Execute(name, args);
}
