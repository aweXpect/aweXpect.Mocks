namespace aweXpect.Mocks;

/// <summary>
///     An invocation of a method.
/// </summary>
public class Invocation(string name, object[] parameters)
{
	/// <summary>
	///     The name of the method.
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///     The parameters of the method.
	/// </summary>
	public object[] Parameters { get; } = parameters;
}
