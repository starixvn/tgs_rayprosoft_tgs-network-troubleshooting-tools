using Microsoft.AspNetCore.Mvc;
using Tgs.Network.Troubleshooter.Tools.Core;

namespace Tgs.Network.Troubleshooter.Tools.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class TelnetController : ControllerBase
{
	/// <summary>
	/// Gets the telnet asynchronous.
	/// </summary>
	/// <param name="telnetClient">The telnet client.</param>
	/// <param name="domain">The domain.</param>
	/// <param name="queryType">Type of the query.</param>
	/// <param name="port">The port.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public ValueTask<string> GetTelnetAsync(
		[FromServices] ITelnetClient telnetClient,
		string domain = "google.com",
		[FromQuery] string queryType = "A",
		[FromQuery] int port = 443)
		=> telnetClient.GetTelnetAsync(
			domain: domain,
			queryType: queryType,
			port: port,
			cancellationToken: HttpContext.RequestAborted);
}
