using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks.Tests.TestHelpers;

public class MyMock<T>(T @object) : Mock<T>(MockBehavior.Default)
{
	public IMockSetup Registration => Setup;
	public override T Object => @object;
}
