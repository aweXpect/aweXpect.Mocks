namespace aweXpect.Mocks.Tests.Dummy;

public interface IExampleRepository
{
	User AddUser(string name);
	bool RemoveUser(Guid id);
	void UpdateUser(Guid id, string newName);

	bool TryDelete(Guid id, out User? user);
}

public record User(Guid Id, string Name);
