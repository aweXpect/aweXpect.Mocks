using System;
using System.Reflection;

namespace aweXpect.Mocks;

/// <summary>
///     Allows registration of method calls and property accesses on a mock.
/// </summary>
public interface IMockRaises
{
	/// <summary>
	///     Raises the event with <paramref name="eventName"/> and the given <paramref name="parameters"/>.
	/// </summary>
	void Raises(string eventName, params object?[] parameters);

	/// <summary>
	/// Associates the specified event <paramref name="method"/> on the <paramref name="target"/> with the event identified by the given <paramref name="name"/>.
	/// </summary>
	void AddEvent(string name, object? target, MethodInfo method);

	/// <summary>
	/// Removes the specified event <paramref name="method"/> on the <paramref name="target"/> from the event identified by the given <paramref name="name"/>.
	/// </summary>
	void RemoveEvent(string name, object? target, MethodInfo method);
}
