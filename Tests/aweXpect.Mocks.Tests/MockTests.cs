using aweXpect.Mocks.Invocations;
using aweXpect.Mocks.Setup;

namespace aweXpect.Mocks.Tests;

public class MockTests
{
	[Fact]
	public async Task Execute_MethodWithoutReturnValue_ShouldIncreaseInvocationCountOfRegisteredSetup()
	{
		IMockSetup sut = new MyMock<string>("foo");
		MethodWithoutReturnValueSetup setup = new("my-method");

		sut.RegisterMethod(setup);

		sut.Execute("my-method");

		await That(setup.InvocationCount).IsEqualTo(1);
	}

	[Fact]
	public async Task Execute_MethodWithReturnValue_ShouldIncreaseInvocationCountOfRegisteredSetupAndReturnRegisteredValue()
	{
		IMockSetup sut = new MyMock<string>("foo");
		MethodWithReturnValueSetup<int> setup = new("my-method");
		setup.Returns(42);

		sut.RegisterMethod(setup);

		int value = sut.Execute<int>("my-method");

		await That(setup.InvocationCount).IsEqualTo(1);
		await That(value).IsEqualTo(42);
	}

	[Fact]
	public async Task Invocations_ShouldReturnAllInvocations()
	{
		var sut = new MyMock<string>("foo");
		IMockSetup mockSetup = sut;

		int value = mockSetup.Execute<int>("my-method");
		mockSetup.Get<int>("my-get-property");
		mockSetup.Set("my-set-property", 42);

		await That(sut.Invocations).HasCount(3);
		await That(sut.Invocations).HasItem()
			.Matching<MethodInvocation>(invocation => invocation.Name == "my-method" && invocation.Parameters.Length == 0);
		await That(sut.Invocations).HasItem()
			.Matching<PropertyGetterInvocation>(invocation => invocation.Name == "my-get-property");
		await That(sut.Invocations).HasItem()
			.Matching<PropertySetterInvocation>(invocation => invocation.Name == "my-set-property" && Equals(invocation.Value, 42));
	}

	[Fact]
	public async Task Get_ShouldIncreaseInvocationCountOfRegisteredSetupAndReturnRegisteredValue()
	{
		IMockSetup sut = new MyMock<string>("foo");
		PropertySetup<int> setup = new();
		setup.InitializeWith(42);

		sut.RegisterProperty("my-property", setup);

		int value = sut.Get<int>("my-property");

		await That(setup.GetterInvocationCount).IsEqualTo(1);
		await That(value).IsEqualTo(42);
	}

	[Fact]
	public async Task Set_ShouldIncreaseInvocationCountOfRegisteredSetupAndReturnRegisteredValue()
	{
		IMockSetup sut = new MyMock<string>("foo");
		PropertySetup<int> setup = new();

		sut.RegisterProperty("my-property", setup);

		sut.Set("my-property", 41);

		await That(setup.SetterInvocationCount).IsEqualTo(1);
	}

	[Fact]
	public async Task Set_ShouldChangeGetValue()
	{
		IMockSetup sut = new MyMock<string>("foo");
		PropertySetup<int> setup = new();
		setup.InitializeWith(42);

		sut.RegisterProperty("my-property", setup);

		sut.Set("my-property", 43);
		var value = sut.Get<int>("my-property");

		await That(value).IsEqualTo(43);
	}

	[Fact]
	public async Task Set_WithoutRegisteredSetup_ShouldChangeGetValue()
	{
		IMockSetup sut = new MyMock<string>("foo");

		sut.Set("my-unregistered-property", 43);
		var value = sut.Get<int>("my-unregistered-property");

		await That(value).IsEqualTo(43);
	}

	[Fact]
	public async Task Set_WithNull_ShouldChangeGetValue()
	{
		IMockSetup sut = new MyMock<string>("foo");
		PropertySetup<int?> setup = new();
		setup.InitializeWith(42);

		sut.RegisterProperty("my-property", setup);

		sut.Set("my-property", null);
		var value = sut.Get<int?>("my-property");

		await That(value).IsNull();
	}

	[Fact]
	public async Task RegisterMethod_AfterExecution_ShouldThrowNotSupportedException()
	{
		IMockSetup sut = new MyMock<string>("foo");
		MethodWithoutReturnValueSetup setup = new("my-method");

		sut.Execute("my-method");

		void Act() => sut.RegisterMethod(setup);

		await That(Act).Throws<NotSupportedException>()
			.WithMessage("You may not register additional setups after the first usage of the mock");
		await That(setup.InvocationCount).IsEqualTo(0);
	}

	[Fact]
	public async Task RegisterProperty_AfterExecution_ShouldThrowNotSupportedException()
	{
		IMockSetup sut = new MyMock<string>("foo");
		PropertySetup<int> setup = new();

		sut.Execute("my-property");

		void Act() => sut.RegisterProperty("my-property", setup);

		await That(Act).Throws<NotSupportedException>()
			.WithMessage("You may not register additional setups after the first usage of the mock");
		await That(setup.GetterInvocationCount).IsEqualTo(0);
		await That(setup.SetterInvocationCount).IsEqualTo(0);
	}

	[Fact]
	public async Task ShouldSupportImplicitOperator()
	{
		MyMock<string> sut = new("foo");

		string value = sut;

		await That(value).IsEqualTo("foo");
	}

	private class MyMock<T>(T @object) : Mock<T>
	{
		public override T Object => @object;
	}
}
