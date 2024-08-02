using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using DnsClient;

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
		[FromServices] ILookupClient lookupClient,
		[FromServices] Ping ping,
		string domain = "google.com",
		[FromQuery] int count = 4)
	{
		var result = await lookupClient.QueryAsync(
			query: domain,
			queryType: QueryType.A,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
		var address = result.Answers.ARecords().FirstOrDefault()?.Address;
		ArgumentException.ThrowIfNullOrWhiteSpace(address?.ToString());

		var messageEntries = new List<string>();
		try
		{
			int pingCount = 0;
			// Ping the host continuously
			do
			{
				PingReply reply = ping.Send(address: address);

				messageEntries.Add(reply != null
					? $"Reply from {reply.Address}: " +
					$"Bytes={reply.Buffer.Length} " +
					$"Time={reply.RoundtripTime}ms " +
					$"TTL={reply.Options?.Ttl}"
					: "Ping failed.");

				pingCount++;

				await Task.Delay(
					millisecondsDelay: 1000,
					cancellationToken: HttpContext.RequestAborted)
					.ConfigureAwait(false);
			} while (pingCount < count);
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
