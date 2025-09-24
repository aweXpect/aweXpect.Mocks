using aweXpect.Mocks.Implementations;

namespace aweXpect.Mocks.Tests;

public sealed class DummyTests
{
	[Fact]
	public async Task XXX()
	{
		bool isCalled = false;
		Mock<IUserRepository> mock = Mock.For<IUserRepository>();
		Mock<IUserService> mock2 = Mock.For<IUserService>();
		mock2.Setup.SaveChanges().Callback(() => isCalled = true);

		IUserService repository = mock2.Object;

		repository.SaveChanges();

		await That(isCalled).IsTrue();
	}
}
