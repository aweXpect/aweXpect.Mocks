using System.Threading;

namespace aweXpect.Mocks.Tests.Dummy;

public interface IUserService
{
	bool IsDisposed { get; set; }
	
	void SaveChanges();
	
	Task SaveChangesAsync(CancellationToken cancellationToken);
}
