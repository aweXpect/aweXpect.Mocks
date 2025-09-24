using System.Threading;

namespace aweXpect.Mocks.Tests;

public interface IUserService
{
	void SaveChanges();
	
	Task SaveChangesAsync(CancellationToken cancellationToken);
}
