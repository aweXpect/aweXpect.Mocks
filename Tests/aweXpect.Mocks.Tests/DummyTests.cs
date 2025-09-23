using System.Threading;

namespace aweXpect.Mocks.Tests;

public sealed class DummyTests
{
	[Fact]
	public async Task XXX()
	{
		Mock<IUserRepository> mock = Mock.For<IUserRepository>();
		Mock<IUserService> mock2 = Mock.For<IUserService>();
		bool isCalled = false;
		//mock2.Setup.SaveChangesAsync(CancellationToken.None).Callback(_ => { isCalled = true; }).Returns(_ => Task.CompletedTask);
		//mock.Setup(m => m.AddUser(With<string>.Matching(x => true))).Returns(true);

		IUserService repository = mock2.Object;

		repository.SaveChanges();

		await That(isCalled).IsTrue();
	}
}
