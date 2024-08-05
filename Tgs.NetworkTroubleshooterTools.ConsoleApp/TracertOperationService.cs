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

		Console.WriteLine($"Tracert {domain}");

		var reply = tracertClient.GetTracertAsync(domain: domain);

		await foreach (var result in reply)
		{
			Console.WriteLine(result);
		}
	}
}
