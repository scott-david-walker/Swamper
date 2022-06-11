namespace Swamper;

public class HttpClientConnection
{
    private readonly Uri _uri;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger = new ConsoleLogger();
    private JobResult _result;
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
        using var response = await _httpClient.GetAsync(_uri);
        _result.Push(response.StatusCode);
        //_logger.Log(response.IsSuccessStatusCode ? "Success" : "Error");
    }

    internal JobResult GetResults()
    {
        return _result;
    }
}
