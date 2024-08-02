using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.NetworkTroubleshooterTools.Core;

public interface INslookupClient
{
	IAsyncEnumerable<string> GetNslookupAsync(string domain = "google.com", string queryType = "A", CancellationToken cancellationToken = default);
}
