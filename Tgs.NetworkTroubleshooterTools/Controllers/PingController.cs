using Microsoft.AspNetCore.Mvc;
using Tgs.NetworkTroubleshooterTools.Core;

namespace Tgs.NetworkTroubleshooterTools.Controllers;

[Route("[controller]")]
[ApiController]
public class PingController : ControllerBase
{
	/// <summary>
	/// Gets the ping asynchronous.
	/// </summary>
	/// <param name="pingClient">The ping client.</param>
	/// <param name="domain">The domain.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public IAsyncEnumerable<string> GetPingAsync(
		[FromServices] IPingClient pingClient,
		string domain = "google.com",
		[FromQuery] int count = 4)
		=> pingClient.GetPingAsync(
			domain: domain,
			count: count,
			cancellationToken: HttpContext.RequestAborted);
}
