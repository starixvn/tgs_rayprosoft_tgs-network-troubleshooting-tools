using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace Tgs.NetworkTroubleshooterTools.Core;

internal class PingClient : IPingClient
{
	private readonly Ping _ping;

	public PingClient(Ping ping)
	{
		_ping = ping ?? throw new ArgumentNullException(nameof(ping));
	}

	public async IAsyncEnumerable<string> GetPingAsync(string domain = "google.com", int count = 4, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var ttl = 0;

		// Ping the host continuously
		do
		{
			PingReply reply = _ping.Send(hostNameOrAddress: domain, timeout: 5000, buffer: new byte[32]);

			yield return reply != null
				? $"Reply from {reply.Address}: " +
				$"Bytes={reply.Buffer.Length} " +
				$"Time={reply.RoundtripTime}ms " +
				$"TTL={reply.Options?.Ttl}"
				: "Ping failed.";

			ttl++;

			await Task.Delay(
				millisecondsDelay: 1000,
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		} while (ttl < count);
	}
}
