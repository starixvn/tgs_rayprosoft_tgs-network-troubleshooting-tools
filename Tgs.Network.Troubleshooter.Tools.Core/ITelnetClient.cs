namespace Tgs.Network.Troubleshooter.Tools.Core;

public interface ITelnetClient
{
	ValueTask<string> GetTelnetAsync(string domain = "google.com", string queryType = "A", int port = 443, CancellationToken cancellationToken = default);
}
