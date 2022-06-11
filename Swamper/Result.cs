using System.Collections.Concurrent;
using System.Net;

namespace Swamper;

public class Result
{
    public Dictionary<HttpStatusCode, int> StatusCodes { get;  } = new();

    public Result ParseAllResults(ConcurrentQueue<JobResult> results)
    {
        while (results.TryDequeue(out var jobResult))
        {
            foreach (var statusCodes in jobResult.StatusCodes)
            {
                DictionaryExtensions<HttpStatusCode>.MergeDictionaries(StatusCodes, jobResult.StatusCodes);
            }
        }
        Console.WriteLine(StatusCodes[HttpStatusCode.OK]);
        return this;
    }
}