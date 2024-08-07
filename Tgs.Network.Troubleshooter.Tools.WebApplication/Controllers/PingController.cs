using Microsoft.AspNetCore.Mvc;
using Tgs.Network.Troubleshooter.Tools.Core;

namespace Tgs.Network.Troubleshooter.Tools.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class PingController : ControllerBase
{
	/// <summary>
	/// Gets the ping asynchronous.
	/// </summary>
	/// <param name="pingClient">The ping client.</param>
	/// <param name="domain">The domain.</param>
	/// <param name="queryType">Type of the query.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public IAsyncEnumerable<string> GetPingAsync(
		[FromServices] IPingClient pingClient,
		string domain = "google.com",
		[FromQuery] string queryType = "A",
		[FromQuery] int count = 4)
		=> pingClient.GetPingAsync(
			domain: domain,
			queryType: queryType,
			count: count,
			cancellationToken: HttpContext.RequestAborted);
}
