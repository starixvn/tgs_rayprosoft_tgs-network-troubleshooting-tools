using Tgs.NetworkTroubleshooterTools.Core;

namespace Tgs.NetworkTroubleshooterTools.ConsoleApp;

internal class TracertOperationService(ITracertClient tracertClient) : IOperationService
{
	/// <summary>
	/// Operations the asynchronous.
	/// </summary>
	public async Task OperationAsync()
	{
		Console.Write("請輸入Domain(google.com):");
		var domain = Console.ReadLine();
		domain = (!string.IsNullOrWhiteSpace(domain)) ? domain : "google.com";

		Console.Write("請輸次數(30):");
		var maxHopsString = Console.ReadLine();
		var maxHops = (!string.IsNullOrWhiteSpace(maxHopsString)) ? int.Parse(maxHopsString) : 30;

		Console.WriteLine($"Tracert {domain}({maxHops})");

		var reply = tracertClient.GetTracertAsync(domain: domain, maxHops);

		await foreach (var result in reply)
		{
			Console.WriteLine(result);
		}
	}
}
