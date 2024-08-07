using Microsoft.AspNetCore.Mvc;
using Tgs.Network.Troubleshooter.Tools.Core;
using Tgs.Network.Troubleshooter.Tools.Core.Models;

namespace Tgs.Network.Troubleshooter.Tools.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class TracertController : ControllerBase
{
	/// <summary>
	/// Gets the tracert asynchronous.
	/// </summary>
	/// <param name="tracertClient">The tracert client.</param>
	/// <param name="domain">The domain.</param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public IAsyncEnumerable<TracertEntry> GetTracertAsync(
		[FromServices] ITracertClient tracertClient,
		string domain = "google.com")
		=> tracertClient.GetTracertAsync(
			domain: domain,
			cancellationToken: HttpContext.RequestAborted);
}
