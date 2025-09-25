namespace aweXpect.Mocks.Tests.Dummy;

public sealed class DummyTests
{
	[Fact]
	public async Task XXX()
	{
		bool isCalled = false;
		Mock<IUserRepository> mock = Mock.For<IUserRepository>();
		mock.Setup.AddUser(With.Any<string>()).Callback(() => isCalled = true).Returns(true);
		Mock<IUserService> mock2 = Mock.For<IUserService>();
		Mock<MyUserRepository> mock3 = Mock.For<MyUserRepository>();
		mock3.Setup.RemoveUser("foo").Callback(() => isCalled = true).Returns(true);
		mock3.Setup.RemoveUser(With.Any<string>()).Returns(false);

		mock3.Setup.Values
			.InitializeWith([1, 2,])
			.OnGet(() => { isCalled = true; })
			.OnSet(value => _ = value);

		MyUserRepository repository = mock3.Object;

		repository.RemoveUser("bar");

		await That(isCalled).IsFalse();
		repository.RemoveUser("foo");
		await That(isCalled).IsTrue();
	}
}
