﻿using aweXpect.Mocks.Setup;
using aweXpect.Mocks.Tests.TestHelpers;

namespace aweXpect.Mocks.Tests.Setup;

public class MethodWithReturnValueSetupTests
{
	public sealed class WithoutParametersTests
	{
		[Theory]
		[InlineData("FooMethod", true)]
		[InlineData("BarMethod", false)]
		public async Task Callback_ForMatchingMethod_ShouldBeExecuted(string calledMethod, bool expectedResult)
		{
			var mock = new MyMock<int>(3);
			bool isCalled = false;
			var sut = new MethodWithReturnValueSetup<int>("FooMethod");
			sut.Callback(() => { isCalled = true; });
			mock.Registration.RegisterMethod(sut);

			mock.Registration.Execute<int>(calledMethod);

			await That(isCalled).IsEqualTo(expectedResult);
		}

		[Theory]
		[InlineData("FooMethod", 42)]
		[InlineData("BarMethod", 0)]
		public async Task Returns_ForMatchingMethod_ShouldReturnExpectedValue(string calledMethod, int expectedValue)
		{
			var mock = new MyMock<int>(3);
			var sut = new MethodWithReturnValueSetup<int>("FooMethod");
			sut.Returns(42);
			mock.Registration.RegisterMethod(sut);

			int result = mock.Registration.Execute<int>(calledMethod);

			await That(result).IsEqualTo(expectedValue);
		}

		[Theory]
		[InlineData("FooMethod", 42)]
		[InlineData("BarMethod", 0)]
		public async Task Returns_WithCallback_ForMatchingMethod_ShouldReturnExpectedValue(string calledMethod, int expectedValue)
		{
			var mock = new MyMock<int>(3);
			var sut = new MethodWithReturnValueSetup<int>("FooMethod");
			sut.Returns(() => 42);
			mock.Registration.RegisterMethod(sut);

			int result = mock.Registration.Execute<int>(calledMethod);

			await That(result).IsEqualTo(expectedValue);
		}
	}
	public sealed class With1ParameterTests
	{
		[Theory]
		[InlineData("FooMethod", true)]
		[InlineData("BarMethod", false)]
		public async Task Callback_ForMatchingMethod_ShouldBeExecuted(string calledMethod, bool expectedResult)
		{
			var mock = new MyMock<int>(3);
			bool isCalled = false;
			var sut = new MethodWithReturnValueSetup<int, string>("FooMethod", With.Any<string>());
			sut.Callback(() => { isCalled = true; });
			mock.Registration.RegisterMethod(sut);

			mock.Registration.Execute<int>(calledMethod, "bar");

			await That(isCalled).IsEqualTo(expectedResult);
		}

		[Theory]
		[InlineData("FooMethod", 42)]
		[InlineData("BarMethod", 0)]
		public async Task Returns_ForMatchingMethod_ShouldReturnExpectedValue(string calledMethod, int expectedValue)
		{
			var mock = new MyMock<int>(3);
			var sut = new MethodWithReturnValueSetup<int, string>("FooMethod", With.Any<string>());
			sut.Returns(42);
			mock.Registration.RegisterMethod(sut);

			int result = mock.Registration.Execute<int>(calledMethod, "bar");

			await That(result).IsEqualTo(expectedValue);
		}

		[Theory]
		[InlineData("FooMethod", 42)]
		[InlineData("BarMethod", 0)]
		public async Task Returns_WithCallback_ForMatchingMethod_ShouldReturnExpectedValue(string calledMethod, int expectedValue)
		{
			var mock = new MyMock<int>(3);
			var sut = new MethodWithReturnValueSetup<int, string>("FooMethod", With.Any<string>());
			sut.Returns(() => 42);
			mock.Registration.RegisterMethod(sut);

			int result = mock.Registration.Execute<int>(calledMethod, "bar");

			await That(result).IsEqualTo(expectedValue);
		}

		[Fact]
		public async Task Returns_WithCallback_ShouldReceiveParameter()
		{
			string receivedParameter = string.Empty;
			var mock = new MyMock<int>(3);
			var sut = new MethodWithReturnValueSetup<int, string>("FooMethod", With.Any<string>());
			sut.Returns(p => {
				receivedParameter = p;
				return 42;
			});
			mock.Registration.RegisterMethod(sut);

			int result = mock.Registration.Execute<int>("FooMethod", "bar");

			await That(result).IsEqualTo(42);
			await That(receivedParameter).IsEqualTo("bar");
		}
	}
}
