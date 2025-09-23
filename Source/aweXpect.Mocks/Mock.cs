

using System;
// ReSharper disable once CheckNamespace
using System.Linq.Expressions;

namespace aweXpect.Mocks;

public abstract class Mock<T>
{
	public abstract T Object { get; }
	public static implicit operator T(Mock<T> mock) => mock.Object;
	
	public Setup<T> Setup(Expression<Action<T>> expression)
	{
		var builder = new Setup.Builder(expression);
		return new Setup<T>(builder);
	}
	
	public Setup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
	{
		var builder = new Setup.Builder(expression!);
		return new Setup<T, TResult>(builder);
	}
}

public class Setup
{
	public bool Matches(string name)
	{
		return _name.Equals(name, StringComparison.Ordinal);
	}

	private string _name;

	public Setup(string name)
	{
		_name = name;
	}
	
	public class Builder
	{
		public Expression _name;

		public Builder(Expression name)
		{
			_name = name;
		}
		
		public Setup Build()
		{
			return new Setup("");
		}

		public void Returns(Func<object> func)
		{
			throw new NotImplementedException();
		}
	}
}

public class Setup<T>
{
	protected Setup.Builder Builder { get; }

	public Setup(Setup.Builder builder)
	{
		Builder = builder;
	}
	
	
}
public class Setup<T, TResult>(Setup.Builder builder) : Setup<T>(builder)
{
	public Setup<T, TResult> Returns(TResult result)
	{
		//Builder.Returns(() => result);
		return this;
	}
}
