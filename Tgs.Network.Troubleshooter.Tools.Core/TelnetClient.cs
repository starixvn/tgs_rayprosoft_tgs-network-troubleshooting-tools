using System.Net;
using System.Net.Sockets;
using DnsClient;
using Microsoft.Extensions.Logging;

namespace Tgs.Network.Troubleshooter.Tools.Core;

internal class TelnetClient(
	ILogger<TelnetClient> logger,
	ILookupClient lookupClient,
	TcpClient tcpClient) : ITelnetClient
{
	public async ValueTask<string> GetTelnetAsync(string domain = "google.com", string queryType = "A", int port = 443, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await lookupClient.QueryAsync(
				query: domain,
				queryType: (QueryType)Enum.Parse(typeof(QueryType), queryType),
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);

			if (!IPAddress.TryParse(result.Answers.ARecords()?.FirstOrDefault()?.Address.ToString(), out var ipAddress))
				throw new ArgumentException(string.Format("{0} is not a valid IP address.", domain));

			await tcpClient.ConnectAsync(ipAddress.ToString(), port)
				.ConfigureAwait(false);

			var timeoutCount = 0;
			var timeoutMaxCount = 15;
			do
			{
				if (tcpClient.Connected)
				{
					return string.Format("正連線到 {0}...開啟到主機的連線， 在連接埠 {1}: 連線成功", domain, port);
				}

				logger.LogWarning("{logWarning}", string.Format("[PORT] {0}:{1} 等待連線{2}秒", domain, port, timeoutCount));

				timeoutCount++;

				await Task.Delay(
					delay: TimeSpan.FromSeconds(1),
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);
			} while (timeoutCount < timeoutMaxCount && !tcpClient.Connected);

			// show result
			return string.Format("正連線到 {0}...無法開啟到主機的連線， 在連接埠 {1}: 逾時連線{2}秒", domain, port, timeoutMaxCount);
		}
		catch
		{
			return string.Format("正連線到 {0}...無法開啟到主機的連線， 在連接埠 {1}: 連線失敗", domain, port);
		}
	}
}
