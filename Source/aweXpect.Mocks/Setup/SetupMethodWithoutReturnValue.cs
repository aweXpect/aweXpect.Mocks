using System;

namespace aweXpect.Mocks.Setup;

public class SetupMethodWithoutReturnValue(string name) : MockSetup
{
	private Action? _callback;

	public SetupMethodWithoutReturnValue Callback(Action callback)
	{
		_callback = callback;
		return this;
	}

	protected override void ExecuteCallback(Invocation invocation) => _callback?.Invoke();

	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
		=> throw new NotSupportedException("The setup does not support return values");

	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 0;
}

public class SetupMethodWithoutReturnValue<T>(string name, MatchParameter match) : MockSetup
{
	private Action<T>? _callback;

	public SetupMethodWithoutReturnValue<T> Callback(Action callback)
	{
		_callback = _ => callback();
		return this;
	}

	public SetupMethodWithoutReturnValue<T> Callback(Action<T> callback)
	{
		_callback = callback;
		return this;
	}

	protected override void ExecuteCallback(Invocation invocation)
	{
		if (invocation.Parameters[0] is T p1)
		{
			_callback?.Invoke(p1);
		}
	}

	protected override TResult GetReturnValue<TResult>(Invocation invocation)
		where TResult : default
		=> throw new NotSupportedException("The setup does not support return values");

	public override bool Matches(Invocation invocation)
		=> invocation.Name.Equals(name) && invocation.Parameters.Length == 1 && match.Matches(invocation.Parameters[0]);
}
