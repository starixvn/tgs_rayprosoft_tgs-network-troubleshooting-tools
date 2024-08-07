using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using DnsClient;

namespace Tgs.Network.Troubleshooter.Tools.Core;

internal class PingClient(
	ILookupClient lookupClient,
	Ping ping) : IPingClient
{
	public async IAsyncEnumerable<string> GetPingAsync(string domain = "google.com", string queryType = "A", int count = 4, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var ttl = 0;

		// Ping the host continuously

		var result = await lookupClient.QueryAsync(
			query: domain,
			queryType: (QueryType)Enum.Parse(typeof(QueryType), queryType),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!IPAddress.TryParse(result.Answers.ARecords()?.FirstOrDefault()?.Address.ToString(), out var ipAddress))
			throw new ArgumentException(string.Format("{0} is not a valid IP address.", domain));

		do
		{
			var reply = await ping.SendPingAsync(ipAddress, timeout: TimeSpan.FromSeconds(5), buffer: null, options: null, cancellationToken: cancellationToken).ConfigureAwait(false);

			yield return reply != null
			? $"Reply from {reply.Address}: " +
			$"Bytes={reply.Buffer.Length} " +
				$"Time={reply.RoundtripTime}ms " +
				$"TTL={reply.Options?.Ttl}"
				: "Ping failed.";

			ttl++;

			await Task.Delay(
				delay: TimeSpan.FromSeconds(1),
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		} while (ttl < count);
	}
}
