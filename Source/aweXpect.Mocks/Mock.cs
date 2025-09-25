using System;
using System.Collections.Generic;
using System.Linq;
using aweXpect.Mocks.Exceptions;
using aweXpect.Mocks.Invocations;
using aweXpect.Mocks.Setup;
using aweXpect.Formatting;
using static aweXpect.Formatting.Format;

namespace aweXpect.Mocks;

/// <summary>
///     A mock for type <typeparamref name="T" />.
/// </summary>
public abstract class Mock<T> : IMockSetup
{
	/// <inheritdoc cref="Mock{T}" />
	protected Mock(MockBehavior mockBehavior)
	{
		_behavior = mockBehavior;
	}

	private readonly List<Invocation> _invocations = [];
	private readonly Dictionary<string, PropertySetup> _propertySetups = [];
	private readonly List<MethodSetup> _setups = [];

	/// <summary>
	/// Gets the behavior settings used by this mock instance.
	/// </summary>
	MockBehavior IMockSetup.Behavior => _behavior;
	private readonly MockBehavior _behavior;

	/// <summary>
	///     The registered invocations of the mock.
	/// </summary>
	public IReadOnlyList<Invocation> Invocations => _invocations.AsReadOnly();

	/// <summary>
	///     Exposes the mocked object instance.
	/// </summary>
	public abstract T Object { get; }

	/// <summary>
	///     Allows setting up the mock.
	/// </summary>
	public MockSetup<T> Setup => new(this);

	/// <inheritdoc cref="IMockSetup.RegisterMethod" />
	void IMockSetup.RegisterMethod(MethodSetup methodSetup)
	{
		if (_invocations.Count > 0)
		{
			throw new NotSupportedException("You may not register additional setups after the first usage of the mock");
		}

		_setups.Add(methodSetup);
	}

	/// <inheritdoc cref="IMockSetup.RegisterProperty" />
	void IMockSetup.RegisterProperty(string propertyName, PropertySetup propertySetup)
	{
		if (_invocations.Count > 0)
		{
			throw new NotSupportedException("You may not register additional setups after the first usage of the mock");
		}

		_propertySetups.Add(propertyName, propertySetup);
	}

	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	TResult IMockSetup.Execute<TResult>(string methodName, params object?[] parameters)
	{
		Invocation invocation = RegisterInvocation(new MethodInvocation(methodName, parameters));

		MethodSetup? matchingSetup = _setups.FirstOrDefault(setup => ((IMethodSetup)setup).Matches(invocation));
		if (matchingSetup is null)
		{
			if (_behavior.ThrowWhenNotSetup)
			{
				throw new MockNotSetupException($"The method '{methodName}({string.Join(",", parameters.Select(x => Formatter.Format(x?.GetType())))})' was invoked without prior setup.");
			}

			return _behavior.DefaultValueGenerator.Generate<TResult>();
		}

		return matchingSetup.Invoke<TResult>(invocation);
	}

	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	void IMockSetup.Execute(string methodName, params object?[] parameters)
	{
		Invocation invocation = RegisterInvocation(new MethodInvocation(methodName, parameters));

		MethodSetup? matchingSetup = _setups.FirstOrDefault(setup => ((IMethodSetup)setup).Matches(invocation));
		matchingSetup?.Invoke(invocation);
	}

	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	void IMockSetup.Set(string propertyName, object? value)
	{
		Invocation invocation = RegisterInvocation(new PropertySetterInvocation(propertyName, value));

		if (!_propertySetups.TryGetValue(propertyName, out PropertySetup? matchingSetup))
		{
			matchingSetup = new PropertySetup.Default();
			_propertySetups.Add(propertyName, matchingSetup);
		}

		matchingSetup.InvokeSetter(invocation, value);
	}

	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	TResult IMockSetup.Get<TResult>(string propertyName)
	{
		Invocation invocation = RegisterInvocation(new PropertyGetterInvocation(propertyName));

		if (!_propertySetups.TryGetValue(propertyName, out PropertySetup? matchingSetup))
		{
			matchingSetup = new PropertySetup.Default();
			_propertySetups.Add(propertyName, matchingSetup);
		}

		return matchingSetup.InvokeGetter<TResult>(invocation);
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

	private Invocation RegisterInvocation(Invocation invocation)
	{
		_invocations.Add(invocation);
		return invocation;
	}
}
