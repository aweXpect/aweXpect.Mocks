using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks;

/// <summary>
///     Allows registration of method calls and property accesses on a mock.
/// </summary>
public interface IMock
{
	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	MethodSetupResult<TResult> Execute<TResult>(string methodName, params object?[] parameters);

	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	MethodSetupResult Execute(string methodName, params object?[] parameters);

	/// <summary>
	///     Accesses the getter of a property.
	/// </summary>
	TResult Get<TResult>(string propertyName);

	/// <summary>
	///     Accesses the setter of a property.
	/// </summary>
	void Set(string propertyName, object? value);
}
