using Tgs.Network.Troubleshooter.Tools.Core.Models;

namespace Tgs.Network.Troubleshooter.Tools.Core;

public interface ITracertClient
{
	IAsyncEnumerable<TracertEntry> GetTracertAsync(string domain = "google.com", int maxHops = 30, CancellationToken cancellationToken = default);
}
