using Tgs.NetworkTroubleshooterTools.Core;

namespace Tgs.NetworkTroubleshooterTools.ConsoleApp;
internal class PingOperationService(IPingClient pingClient) : IOperationService
{
	/// <summary>
	/// Operations the asynchronous.
	/// </summary>
	public async Task OperationAsync()
	{
		Console.Write("請輸入Domain(google.com):");
		var domain = Console.ReadLine();
		domain = (!string.IsNullOrWhiteSpace(domain)) ? domain : "google.com";

		Console.Write("請輸次數(4):");
		var countString = Console.ReadLine();
		var count = (!string.IsNullOrWhiteSpace(countString)) ? int.Parse(countString) : 4;

		Console.WriteLine($"Ping {domain}");

		var reply = pingClient.GetPingAsync(domain: domain, count: count);

		await foreach (var result in reply)
		{
			Console.WriteLine(result);
		}
	}
}
