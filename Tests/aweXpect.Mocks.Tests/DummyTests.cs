namespace aweXpect.Mocks.Tests;

public sealed class DummyTests
{
	[Fact]
	public async Task XXX()
	{
		Mock<IUserRepository> mock = Mock.For<IUserRepository>();
		Mock<IUserService> mock2 = Mock.For<IUserService>();

		IUserRepository repository = mock.Object;

		var result = repository.AddUser("foo");

		await That(result).IsTrue();
	}
}
