using System.Collections.Concurrent;
using System.Net;

namespace Swamper;

internal class JobResult
{
    internal ConcurrentDictionary<HttpStatusCode, int> StatusCodes { get; } = new();
    internal List<long> MillisecondsPerRequest { get; } = new();
    
    internal void Push(HttpStatusCode statusCode, long millisecondsPerRequest)
    {
        DictionaryExtensions<HttpStatusCode>.AddOrUpdateCount(StatusCodes, statusCode);
        MillisecondsPerRequest.Add(millisecondsPerRequest);
    }
}