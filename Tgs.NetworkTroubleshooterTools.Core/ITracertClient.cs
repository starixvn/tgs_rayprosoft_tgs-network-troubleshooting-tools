using Tgs.NetworkTroubleshooterTools.Core.Models;

namespace Tgs.NetworkTroubleshooterTools.Core;

public interface ITracertClient
{
	IAsyncEnumerable<TracertEntry> GetTracertAsync(string domain = "google.com", int maxHops = 30, CancellationToken cancellationToken = default);
}
