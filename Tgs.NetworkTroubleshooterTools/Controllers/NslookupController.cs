using DnsClient;

using Microsoft.AspNetCore.Mvc;

namespace Tgs.NetworkTroubleshooterTools.Controllers;

[Route("[controller]")]
[ApiController]
public class NslookupController : ControllerBase
{
	[HttpGet("{domain}")]
	public async IAsyncEnumerable<string> GetNslookup(
		[FromServices] ILookupClient lookupClient,
		string domain = "google.com",
		[FromQuery] string queryType = "A")
	{
		var result = await lookupClient.QueryAsync(
			query: domain,
			queryType: (QueryType)Enum.Parse(typeof(QueryType), queryType),
			cancellationToken: HttpContext.RequestAborted)
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
