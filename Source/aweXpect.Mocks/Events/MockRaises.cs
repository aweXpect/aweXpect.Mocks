﻿using System;
using System.Reflection;
using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks.Events;

/// <summary>
///     Allows raising events on the mock.
/// </summary>
public class MockRaises<T>(MockSetup<T> setup) : IMockRaises
{
	/// <inheritdoc cref="IMockRaises.Raises(string, object?[])" />
	void IMockRaises.Raises(string eventName, params object?[] parameters)
	{
		foreach (var(target, method) in setup.GetEventHandlers(eventName))
		{
			method.Invoke(target, parameters);
		}
	}

	/// <inheritdoc cref="IMockRaises.AddEvent(string, object?, MethodInfo)" />
	void IMockRaises.AddEvent(string name, object? target, MethodInfo method)
	{
		setup.AddEvent(name, target, method);
	}

	/// <inheritdoc cref="IMockRaises.RemoveEvent(string, object?, MethodInfo)" />
	void IMockRaises.RemoveEvent(string name, object? target, MethodInfo method)
	{
		setup.RemoveEvent(name, target, method);
	}
}
