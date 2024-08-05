namespace Tgs.NetworkTroubleshooterTools.Core;

public interface INslookupClient
{
	IAsyncEnumerable<string> GetNslookupAsync(string domain = "google.com", string queryType = "A", CancellationToken cancellationToken = default);
}
