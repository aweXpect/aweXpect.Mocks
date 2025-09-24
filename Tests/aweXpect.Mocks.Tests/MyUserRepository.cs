using System.Collections.Generic;

namespace aweXpect.Mocks.Tests;

public class MyUserRepository
{
	private readonly List<string> _users = new();

	public void AddUser(string email)
		=> _users.Add(email);

	public virtual bool RemoveUser(string email)
		=> _users.Remove(email);
}
