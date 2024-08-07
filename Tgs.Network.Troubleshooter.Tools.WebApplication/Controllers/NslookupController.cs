using DnsClient;

using Microsoft.AspNetCore.Mvc;
using Tgs.Network.Troubleshooter.Tools.Core;

namespace Tgs.Network.Troubleshooter.Tools.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class NslookupController : ControllerBase
{
	/// <summary>
	///
	/// </summary>
	/// <param name="nslookupClient"></param>
	/// <param name="domain"></param>
	/// <param name="queryType"></param>
	/// <returns></returns>
	[HttpGet("{domain}")]
	public IAsyncEnumerable<string> GetNslookupAsync(
		[FromServices] INslookupClient nslookupClient,
		string domain = "google.com",
		[FromQuery] string queryType = "A")
		=> nslookupClient.GetNslookupAsync(
			domain: domain,
			queryType: queryType,
			cancellationToken: HttpContext.RequestAborted);
}
