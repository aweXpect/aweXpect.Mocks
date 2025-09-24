namespace aweXpect.Mocks.Setup;

/// <summary>
///     Allows registration of <see cref="MockSetup" /> in the mock.
/// </summary>
public interface IMockSetup
{
	/// <summary>
	///     Registers the <paramref name="mockSetup" /> in the mock.
	/// </summary>
	void RegisterSetup(MockSetup mockSetup);
	
	/// <summary>
	///     Executes a method and gets the setup return value.
	/// </summary>
	TResult Execute<TResult>(string name, params object[] args);
	
	/// <summary>
	///     Executes a method returning <see langword="void" />.
	/// </summary>
	void Execute(string name, params object[] args);
}
