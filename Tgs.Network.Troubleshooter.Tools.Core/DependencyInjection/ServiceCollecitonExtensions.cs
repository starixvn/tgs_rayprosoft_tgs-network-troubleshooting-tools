using System.Net.NetworkInformation;
using System.Net.Sockets;
using DnsClient;
using Tgs.Network.Troubleshooter.Tools.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
	public static IServiceCollection AddNetworkTroubleshooterToolsCore(this IServiceCollection services)
		=> services.AddTransient<Ping>()
		.AddTransient<TcpClient>()
		.AddTransient<ILookupClient, LookupClient>()
		.AddTransient<INslookupClient, NslookupClient>()
		.AddTransient<IPingClient, PingClient>()
		.AddTransient<ITracertClient, TracertClient>()
		.AddTransient<ITelnetClient, TelnetClient>();
}
