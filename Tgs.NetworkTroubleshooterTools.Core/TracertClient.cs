using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Tgs.NetworkTroubleshooterTools.Core.Models;

namespace Tgs.NetworkTroubleshooterTools.Core;

internal class TracertClient : ITracertClient
{
	private readonly Ping _ping;

	public TracertClient(Ping ping)
	{
		_ping = ping ?? throw new ArgumentNullException(nameof(ping));
	}

	public async IAsyncEnumerable<TracertEntry> GetTracertAsync(string domain = "google.com", int maxHops = 30, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var ipAddress = (Dns.GetHostAddresses(domain).FirstOrDefault()?.ToString())
			?? throw new ArgumentException(string.Format("{0} is not a valid IP address.", domain));

		// Ensure that the argument address is valid.
		if (!IPAddress.TryParse(ipAddress, out IPAddress? address))
			throw new ArgumentException(string.Format("{0} is not a valid IP address.", ipAddress));

		PingOptions pingOptions = new PingOptions(1, true);
		Stopwatch pingReplyTime = new Stopwatch();
		PingReply reply;
		do
		{
			pingReplyTime.Start();
			reply = _ping.Send(address, timeout: 5000, buffer: new byte[32], options: pingOptions);
			pingReplyTime.Stop();

			string hostname = string.Empty;
			if (reply.Address != null && reply.Address.ToString() != IPAddress.Any.ToString() && reply.Address.ToString() != IPAddress.IPv6Any.ToString())
			{
				try
				{
					hostname = (await Dns.GetHostEntryAsync(reply.Address).ConfigureAwait(false)).HostName;    // Retrieve the hostname for the replied address.
				}
				catch (SocketException) { /* No host available for that address. */ }
			}

			// Return out TracertEntry object with all the information about the hop.
			yield return new TracertEntry()
			{
				HopID = pingOptions.Ttl,
				Address = reply.Address == null ? "N/A" : reply.Address.ToString(),
				Hostname = hostname,
				ReplyTime = pingReplyTime.ElapsedMilliseconds,
				ReplyStatus = reply.Status
			};

			pingOptions.Ttl++;
			pingReplyTime.Reset();
		}
		while (reply.Status != IPStatus.Success && pingOptions.Ttl <= maxHops);
	}
}
