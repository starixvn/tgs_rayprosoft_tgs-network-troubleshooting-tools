namespace Tgs.Network.Troubleshooter.Tools.Core;

public interface IPingClient
{
	IAsyncEnumerable<string> GetPingAsync(string domain = "google.com", string queryType = "A", int count = 4, CancellationToken cancellationToken = default);
}
