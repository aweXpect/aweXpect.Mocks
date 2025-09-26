using System;
using System.Collections.Generic;
using System.Linq;
using aweXpect.Mocks.Invocations;

namespace aweXpect.Mocks.Setup;

/// <summary>
///     Sets up the <see cref="Mock{T}" /> <paramref name="mock" />
/// </summary>
public class MockSetup<T>(Mock<T> mock) : IMockSetup
{
	private readonly Dictionary<string, PropertySetup> _propertySetups = [];
	private readonly List<MethodSetup> _methodSetups = [];

	/// <inheritdoc cref="IMockSetup.RegisterMethod" />
	void IMockSetup.RegisterMethod(MethodSetup methodSetup)
	{
		if (mock.Invoked.IsAlreadyInvoked)
		{
			throw new NotSupportedException("You may not register additional setups after the first usage of the mock");
		}

		_methodSetups.Add(methodSetup);
	}

	/// <inheritdoc cref="IMockSetup.RegisterProperty" />
	void IMockSetup.RegisterProperty(string propertyName, PropertySetup propertySetup)
	{
		if (mock.Invoked.IsAlreadyInvoked)
		{
			throw new NotSupportedException("You may not register additional setups after the first usage of the mock");
		}

		_propertySetups.Add(propertyName, propertySetup);
	}

	/// <summary>
	///     Retrieves the first method setup that matches the specified <paramref name="invocation" />,
	///     or returns <see langword="null" /> if no matching setup is found.
	/// </summary>
	internal MethodSetup? GetMethodSetup(Invocation invocation)
	{
		return _methodSetups.FirstOrDefault(setup => ((IMethodSetup)setup).Matches(invocation));
	}

	/// <summary>
	///     Retrieves the setup configuration for the specified property name, creating a default setup if none exists.
	/// </summary>
	/// <remarks>
	///     If the specified property name does not have an associated setup, a default configuration is
	///     created and stored for future retrievals, so that getter and setter work in tandem.
	/// </remarks>
	internal PropertySetup GetPropertySetup(string propertyName)
	{
		if (!_propertySetups.TryGetValue(propertyName, out PropertySetup? matchingSetup))
		{
			matchingSetup = new PropertySetup.Default();
			_propertySetups.Add(propertyName, matchingSetup);
		}

		return matchingSetup;
	}
}
