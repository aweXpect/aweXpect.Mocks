using System;
using System.Collections.Generic;
using System.Linq;
using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks;

/// <summary>
///     A mock for type <typeparamref name="T" />.
/// </summary>
public abstract class Mock<T> : IMockSetup
{
	private readonly List<Invocation> _invocations = [];
	private readonly List<MockSetup> _setups = [];

	/// <summary>
	///     Exposes the mocked object instance.
	/// </summary>
	public abstract T Object { get; }

	/// <summary>
	///     Allows setting up the mock.
	/// </summary>
	public MockSetup<T> Setup => new(this);

	/// <inheritdoc cref="IMockSetup.RegisterSetup(MockSetup)" />
	void IMockSetup.RegisterSetup(MockSetup mockSetup)
	{
		if (_invocations.Count > 0)
		{
			throw new NotSupportedException("You may not register additional setups after the first usage of the mock");
		}

		_setups.Add(mockSetup);
	}

	/// <summary>
	///     Implicitly converts the mock to the mocked object instance.
	/// </summary>
	/// <remarks>
	///     This does not work implicitly (but only with an explicit cast) for interfaces due to
	///     a limitation of the C# language.
	/// </remarks>
	public static implicit operator T(Mock<T> mock)
	{
		return mock.Object;
	}

	private Invocation RegisterInvocation(string name, object[] parameters)
	{
		// TODO: Create and register invocation
		Invocation invocation = new(name, parameters);
		_invocations.Add(invocation);
		return invocation;
	}

	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	TResult IMockSetup.Execute<TResult>(string name, params object[] args)
	{
		Invocation invocation = RegisterInvocation(name, args);

		MockSetup? matchingSetup = _setups.FirstOrDefault(setup => setup.Matches(invocation));
		if (matchingSetup is null)
		{
			//TODO: Throw exception? maybe depending on a behavior setting?
			return default!;
		}

		return matchingSetup.Invoke<TResult>(invocation);
	}

	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	void IMockSetup.Execute(string name, params object[] args)
	{
		Invocation invocation = RegisterInvocation(name, args);

		MockSetup? matchingSetup = _setups.FirstOrDefault(setup => setup.Matches(invocation));
		matchingSetup?.Invoke(invocation);
	}
}
