using System;

namespace aweXpect.Mocks.Exceptions;

public class MockNotSetupException : MockException
{
	public MockNotSetupException(string message) : base(message)
	{
	}

	public MockNotSetupException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
