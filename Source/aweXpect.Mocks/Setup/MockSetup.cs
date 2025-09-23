using System.Threading;

namespace aweXpect.Mocks.Setup;

public abstract class MockSetup
{
	private int _invocationCount;
	public int InvocationCount => _invocationCount;

	public TResult Invoke<TResult>(Invocation invocation)
	{
		Interlocked.Increment(ref _invocationCount);
		ExecuteCallback(invocation);
		return GetReturnValue<TResult>(invocation);
	}

	public void Invoke(Invocation invocation)
	{
		Interlocked.Increment(ref _invocationCount);
		ExecuteCallback(invocation);
	}

	protected abstract void ExecuteCallback(Invocation invocation);

	protected abstract TResult GetReturnValue<TResult>(Invocation invocation);

	public abstract bool Matches(Invocation invocation);
}

public class MockSetup<T>(Mock<T> mock) : IMockSetup
{
	/// <inheritdoc cref="IMockSetup.RegisterSetup(MockSetup)" />
	void IMockSetup.RegisterSetup(MockSetup mockSetup) => ((IMockSetup)mock).RegisterSetup(mockSetup);
}
