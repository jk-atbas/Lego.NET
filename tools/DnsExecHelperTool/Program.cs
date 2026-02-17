using System.Net.Http.Json;

if (args.Length < 2)
{
	Console.Error.WriteLine("Usage: <present|cleanup> <fqdn> [value]");

	return 2;
}

var action = args[0];
var fqdn = args[1];
var value = args.Length >= 3 ? args[2] : "";

var baseUrl = Environment.GetEnvironmentVariable("CHALLTESTSRV_URL");

if (string.IsNullOrWhiteSpace(baseUrl))
{
	Console.Error.WriteLine("CHALLTESTSRV_URL env var is missing (e.g. http://localhost:12345)");

	return 3;
}

using var http = new HttpClient();
http.BaseAddress = new Uri(baseUrl, UriKind.Absolute);

if (action.Equals("present", StringComparison.OrdinalIgnoreCase))
{
	var payload = new { host = fqdn, value };
	var resp = await http.PostAsJsonAsync("/set-txt", payload);
	resp.EnsureSuccessStatusCode();

	return 0;
}

if (action.Equals("cleanup", StringComparison.OrdinalIgnoreCase))
{
	var payload = new { host = fqdn };
	var resp = await http.PostAsJsonAsync("/clear-txt", payload);
	resp.EnsureSuccessStatusCode();

	return 0;
}

Console.Error.WriteLine($"Unknown action: {action}");

return 2;
