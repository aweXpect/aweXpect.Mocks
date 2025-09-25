namespace aweXpect.Mocks.Setup;

/// <summary>
///     Allows registration of <see cref="MethodSetup" /> in the mock.
/// </summary>
public interface IMockSetup
{
	/// <summary>
	/// Gets the behavior settings used by this mock instance.
	/// </summary>
	MockBehavior Behavior { get; }

	/// <summary>
	///     Registers the <paramref name="methodSetup" /> in the mock.
	/// </summary>
	void RegisterMethod(MethodSetup methodSetup);

	/// <summary>
	///     Registers the <paramref name="propertySetup" /> in the mock.
	/// </summary>
	void RegisterProperty(string propertyName, PropertySetup propertySetup);

	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	TResult Execute<TResult>(string methodName, params object?[] parameters);

	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	void Execute(string methodName, params object?[] parameters);

	/// <summary>
	///     Accesses the getter of a property.
	/// </summary>
	TResult Get<TResult>(string propertyName);

	/// <summary>
	///     Accesses the setter of a property.
	/// </summary>
	void Set(string propertyName, object? value);
}
