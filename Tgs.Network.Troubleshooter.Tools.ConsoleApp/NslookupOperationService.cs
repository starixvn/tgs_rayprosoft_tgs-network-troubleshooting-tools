using Tgs.Network.Troubleshooter.Tools.Core;

namespace Tgs.Network.Troubleshooter.Tools.ConsoleApp;

internal class NslookupOperationService(INslookupClient nslookupClient) : IOperationService
{
	/// <summary>
	/// Operations the asynchronous.
	/// </summary>
	public async Task OperationAsync()
	{
		Console.Write("請輸入Domain(google.com):");
		var domain = Console.ReadLine();
		domain = (!string.IsNullOrWhiteSpace(domain)) ? domain : "google.com";

		Console.Write("請輸入Domain Type(A):");
		var queryType = Console.ReadLine();
		queryType = (!string.IsNullOrWhiteSpace(queryType)) ? queryType : "A";

		Console.WriteLine($"Nslookup {domain}({queryType})");

		var reply = nslookupClient.GetNslookupAsync(domain: domain, queryType: queryType);

		await foreach (var result in reply)
		{
			Console.WriteLine(result);
		}
	}
}
