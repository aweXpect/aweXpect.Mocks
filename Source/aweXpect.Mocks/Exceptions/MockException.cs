using System;

namespace aweXpect.Mocks.Exceptions;

public abstract class MockException : Exception
{
	protected MockException(string message) : base(message)
	{
	}

	protected MockException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
