namespace Tgs.Network.Troubleshooter.Tools.Core;

public interface IPingClient
{
	IAsyncEnumerable<string> GetPingAsync(string domain = "google.com", int count = 4, CancellationToken cancellationToken = default);
}
