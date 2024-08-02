using System.Net.NetworkInformation;
using DnsClient;

using Microsoft.AspNetCore.Mvc;

namespace Tgs.NetworkTroubleshooterTools.Controllers;

[Route("[controller]")]
[ApiController]
public class PingController : ControllerBase
{
	/// <summary>
	/// Gets the ping.
	/// </summary>
	/// <param name="ping">The ping.</param>
	/// <param name="domain">The domain.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public async IAsyncEnumerable<string> GetPing(
		[FromServices] Ping ping,
		string domain = "google.com",
		[FromQuery] int count = 4)
	{
		var ttl = 0;
		var messageEntries = new List<string>();
		try
		{
			// Ping the host continuously
			do
			{
				PingReply reply = ping.Send(hostNameOrAddress: domain, timeout: 5000, buffer: new byte[32]);

				messageEntries.Add(reply != null
					? $"Reply from {reply.Address}: " +
					$"Bytes={reply.Buffer.Length} " +
					$"Time={reply.RoundtripTime}ms " +
					$"TTL={reply.Options?.Ttl}"
					: "Ping failed.");

				ttl++;

				await Task.Delay(
					millisecondsDelay: 1000,
					cancellationToken: HttpContext.RequestAborted)
					.ConfigureAwait(false);
			} while (ttl <= count);
		}
		catch (Exception ex)
		{
			messageEntries.Add($"Error pinging {domain}: {ex.Message}");
		}

		foreach (var pingEntry in messageEntries)
		{
			yield return pingEntry;
		}
	}
}
