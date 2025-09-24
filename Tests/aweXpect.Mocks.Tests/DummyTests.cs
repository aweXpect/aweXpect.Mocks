using aweXpect.Mocks.Implementations;
using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks.Tests;

public sealed class DummyTests
{
	[Fact]
	public async Task XXX()
	{
		bool isCalled = false;
		Mock<IUserRepository> mock = Mock.For<IUserRepository>();
		Mock<IUserService> mock2 = Mock.For<IUserService>();
		var mock3 = Mock.For<MyUserRepository>();
		mock3.Setup.RemoveUser("foo").Callback(() => isCalled = true).Returns(true);

		var repository = mock3.Object;

		repository.RemoveUser("bar");

		await That(isCalled).IsFalse();
		repository.RemoveUser("foo");
		await That(isCalled).IsTrue();
	}
}
