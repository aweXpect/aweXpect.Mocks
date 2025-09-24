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
}
