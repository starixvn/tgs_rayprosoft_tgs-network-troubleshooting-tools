using System.Net.NetworkInformation;
using DnsClient;
using Tgs.Network.Troubleshooter.Tools.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
	public static IServiceCollection AddNetworkTroubleshooterToolsCore(this IServiceCollection services)
		=> services.AddSingleton<Ping>()
		.AddSingleton<ILookupClient, LookupClient>()
		.AddSingleton<INslookupClient, NslookupClient>()
		.AddSingleton<IPingClient, PingClient>()
		.AddSingleton<ITracertClient, TracertClient>();
}
