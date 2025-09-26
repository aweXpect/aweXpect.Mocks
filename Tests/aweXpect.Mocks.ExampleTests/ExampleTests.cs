﻿using aweXpect.Mocks.Tests.Dummy;

namespace aweXpect.Mocks.ExampleTests;

public class ExampleTests
{
	[Fact]
	public async Task WithAny_ShouldAlwaysMatch()
	{
		var id = Guid.NewGuid();
		var mock = Mock.For<IExampleRepository>();

		mock.Setup.AddUser(With.Any<string>()).Returns(new User(id, "Alice"));

		var result = mock.Object.AddUser("Bob");

		await That(result).IsEqualTo(new User(id, "Alice"));
		await That(mock.Invoked.AddUser("Bob").Invocations).HasCount(1);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public async Task WithOut_ShouldSupportOutParameter(bool returnValue)
	{
		var id = Guid.NewGuid();
		var mock = Mock.For<IExampleRepository>();

		mock.Setup.TryDelete(With.Any<Guid>(), With.Out<User?>(() => new User(id, "Alice"))).Returns(returnValue);

		var result = mock.Object.TryDelete(id, out var deletedUser);

		await That(deletedUser).IsEqualTo(new User(id, "Alice"));
		await That(result).IsEqualTo(returnValue);
		await That(mock.Invoked.TryDelete(id, With.Out<User?>()).Invocations).HasCount(1);
	}

	[Theory]
	[InlineData("Alice", true)]
	[InlineData("Bob", false)]
	public async Task WithMatching_ShouldAlwaysMatch(string name, bool expectResult)
	{
		var id = Guid.NewGuid();
		var mock = Mock.For<IExampleRepository>();

		mock.Setup.AddUser(With<string>.Matching(x => x == "Alice")).Returns(new User(id, "Alice"));

		var result = mock.Object.AddUser(name);

		await That(result).IsEqualTo(expectResult ? new User(id, "Alice") : null);
		await That(mock.Invoked.AddUser(name).Invocations).HasCount(1);
	}
}
