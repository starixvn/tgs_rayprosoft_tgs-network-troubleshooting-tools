using System.Runtime.CompilerServices;
using DnsClient;

namespace Tgs.NetworkTroubleshooterTools.Core;

internal class NslookupClient : INslookupClient
{
	private readonly ILookupClient _lookupClient;

	public NslookupClient(ILookupClient lookupClient)
	{
		_lookupClient = lookupClient ?? throw new ArgumentNullException(nameof(lookupClient));
	}

	public async IAsyncEnumerable<string> GetNslookupAsync(string domain = "google.com", string queryType = "A", [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var result = await _lookupClient.QueryAsync(
			query: domain,
			queryType: (QueryType)Enum.Parse(typeof(QueryType), queryType),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
		if (result.Answers.ARecords().Any())
		{
			foreach (var aRecord in result.Answers.ARecords())
			{
				yield return aRecord.ToString();
			}
		}
		else
		{
			yield return $"UnKnown 找不到 {domain}: Non-existent domain";
		}
	}
}
