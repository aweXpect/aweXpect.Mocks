namespace aweXpect.Mocks.Setup;

/// <summary>
///     Sets up the <see cref="Mock{T}" /> <paramref name="mock" />
/// </summary>
public class MockSetup<T>(Mock<T> mock) : IMockSetup
{
	/// <inheritdoc cref="IMockSetup.Behavior" />
	MockBehavior IMockSetup.Behavior
		=> ((IMockSetup)mock).Behavior;

	/// <inheritdoc cref="IMockSetup.RegisterMethod" />
	void IMockSetup.RegisterMethod(MethodSetup methodSetup)
		=> ((IMockSetup)mock).RegisterMethod(methodSetup);

	/// <inheritdoc cref="IMockSetup.RegisterProperty" />
	void IMockSetup.RegisterProperty(string propertyName, PropertySetup propertySetup)
		=> ((IMockSetup)mock).RegisterProperty(propertyName, propertySetup);

	/// <inheritdoc cref="IMockSetup.Execute{TResult}(string, object[])" />
	TResult IMockSetup.Execute<TResult>(string methodName, params object?[] parameters)
		=> ((IMockSetup)mock).Execute<TResult>(methodName, parameters);

	/// <inheritdoc cref="IMockSetup.Execute(string, object[])" />
	void IMockSetup.Execute(string methodName, params object?[] parameters)
		=> ((IMockSetup)mock).Execute(methodName, parameters);

	/// <inheritdoc cref="IMockSetup.Get{TResult}(string)" />
	TResult IMockSetup.Get<TResult>(string propertyName)
		=> ((IMockSetup)mock).Get<TResult>(propertyName);

	/// <inheritdoc cref="IMockSetup.Set(string, object?)" />
	void IMockSetup.Set(string propertyName, object? value)
		=> ((IMockSetup)mock).Set(propertyName, value);
}
