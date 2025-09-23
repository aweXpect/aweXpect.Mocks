namespace aweXpect.Mocks;

public class Invocation
{
	public Invocation(string name, object[] parameters)
	{
		Name = name;
		Parameters = parameters;
	}

	public string Name { get; }

	public object[] Parameters { get; }
}
