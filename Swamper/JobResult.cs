using System.Net;

namespace Swamper;

public class JobResult
{
    public Dictionary<HttpStatusCode, int> StatusCodes { get; } = new();
    public List<long> MillisecondsPerRequest { get; } = new();

    

    public void Push(HttpStatusCode statusCode, long millisecondsPerRequest)
    {
        DictionaryExtensions<HttpStatusCode>.AddOrUpdateCount(StatusCodes, statusCode);
        MillisecondsPerRequest.Add(millisecondsPerRequest);
    }
}