// See https://aka.ms/new-console-template for more information

using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.NetworkTroubleshooterTools.ConsoleApp;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddNetworkTroubleshooterToolsCore();
builder.Services.AddKeyedSingleton<IOperationService, NslookupOperationService>("Nslookup");
builder.Services.AddKeyedSingleton<IOperationService, PingOperationService>("Ping");
builder.Services.AddKeyedSingleton<IOperationService, TracertOperationService>("Tracert");

var app = builder.Build();

var operater = OperationType.Exit;

var dict = Enum.GetValues(typeof(OperationType))
			   .Cast<OperationType>()
			   .ToDictionary(t => (int)t, t => t.ToString());

var options = new StringBuilder();
foreach (var dateType in dict)
{
	_ = options.AppendLine($"({dateType.Key}).{dateType.Value}");
}

do
{
	Console.WriteLine(options);
	Console.Write("選擇操作模式:");

	var operaterString = Console.ReadLine();

	operater = (!string.IsNullOrWhiteSpace(operaterString)) ? ((OperationType)Convert.ToInt32(operaterString)) : OperationType.Exit;

	if (operater != OperationType.Exit)
	{
		Console.WriteLine("操作模式:{0}", operater.ToString());

		var operationService = app.Services.GetKeyedService<IOperationService>(operater.ToString()) ?? throw new ArgumentNullException(operater.ToString());
		await operationService.OperationAsync().ConfigureAwait(false);
	}
} while (operater != OperationType.Exit);
