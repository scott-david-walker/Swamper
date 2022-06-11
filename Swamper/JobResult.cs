using System.Net;

namespace Swamper;

public class JobResult
{
    public Dictionary<HttpStatusCode, int> StatusCodes { get; } = new();

    public void Push(HttpStatusCode statusCode)
    {
        DictionaryExtensions<HttpStatusCode>.AddOrUpdateCount(StatusCodes, statusCode);
    }
}