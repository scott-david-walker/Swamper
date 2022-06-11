using System.Diagnostics;

namespace Swamper;

public class HttpClientConnection
{
    private readonly Uri _uri;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger = new ConsoleLogger();
    private JobResult? _result;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public HttpClientConnection(Uri uri, HttpClient  httpClient)
    {
        _uri = uri;
        _httpClient = httpClient;
    }
    internal ValueTask<HttpClientConnection> Initialise()
    {
        _result = new JobResult();
        return new ValueTask<HttpClientConnection>(Task.FromResult(this));
    }

    internal async ValueTask Connect()
    {
        _stopwatch.Restart();
        using var response = await _httpClient.GetAsync(_uri);
        _result!.Push(response.StatusCode, _stopwatch.ElapsedMilliseconds);
    }

    internal JobResult GetResults()
    {
        return _result!;
    }
}
