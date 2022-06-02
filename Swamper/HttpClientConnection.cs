namespace Swamper;

public class HttpClientConnection
{
    private readonly Uri _uri;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger = new ConsoleLogger();
    public HttpClientConnection(Uri uri)
    {
        _uri = uri;
        _httpClient = new HttpClient();
    }
    internal ValueTask<HttpClientConnection> Initialise()
    {
        return new ValueTask<HttpClientConnection>(Task.FromResult(this));
    }

    internal async ValueTask Connect()
    {
        using var response = await _httpClient.GetAsync(_uri);
        _logger.Log(response.IsSuccessStatusCode ? "Success" : "Error");
    }
}
