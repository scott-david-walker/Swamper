using System.Diagnostics;

namespace Swamper;

public class HttpClientConnection
{
    private readonly HttpRequestMessage _httpRequestMessage;
    private readonly HttpClient _httpClient;
    private JobResult? _result;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public HttpClientConnection(HttpRequestMessage httpRequestMessage, HttpClient httpClient)
    {
        _httpRequestMessage = httpRequestMessage;
        _httpClient = httpClient;
    }
    
    internal ValueTask<HttpClientConnection> Initialise()
    {
        return new ValueTask<HttpClientConnection>(
            Task.FromResult(new HttpClientConnection(_httpRequestMessage, _httpClient)
        {
            _result = new JobResult()
        }));
    }

    internal async ValueTask Connect()
    {
        _stopwatch.Restart();
        using var response = await _httpClient.SendAsync(await HttpRequestCloner.Clone(_httpRequestMessage));
        _result!.Push(response.StatusCode, _stopwatch.ElapsedMilliseconds);
    }

    internal JobResult GetResults()
    {
        return _result!;
    }
}
