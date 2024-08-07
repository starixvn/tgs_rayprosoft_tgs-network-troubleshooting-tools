namespace Tgs.Network.Troubleshooter.Tools.Core;

public interface INslookupClient
{
	IAsyncEnumerable<string> GetNslookupAsync(string domain = "google.com", string queryType = "A", CancellationToken cancellationToken = default);
}
