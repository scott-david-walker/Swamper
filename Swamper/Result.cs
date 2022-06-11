using System.Collections.Concurrent;
using System.Net;

namespace Swamper;

public class Result
{
    internal ConcurrentDictionary<HttpStatusCode, int> StatusCodes { get;  } = new();
    internal double AverageTimePerRequest => CalculateAverage(); 

    internal int NumberOfRequests => StatusCodes.Select(t => t.Value).Sum();
    internal double MaximumRequestTime { get; set; }
    internal double MinimumRequestTime { get; set; } = 1000000; // Large number so 1st request should be lower
    internal double MedianRequestTime => CalculateMedian();
    private long OverallMilliseconds { get; set; }
    private List<long> MillisecondsPerRequest { get; } = new();

    internal Result ParseAllResults(ConcurrentQueue<JobResult> results)
    {
        while (results.TryDequeue(out var jobResult))
        {
            foreach (var _ in jobResult.StatusCodes)
            {
                DictionaryExtensions<HttpStatusCode>.MergeDictionaries(StatusCodes, jobResult.StatusCodes);
            }
            MillisecondsPerRequest.AddRange(jobResult.MillisecondsPerRequest);
            CalculateMaximumAndMinimum(jobResult.MillisecondsPerRequest);
            OverallMilliseconds += jobResult.MillisecondsPerRequest.Sum();
            
        }
        return this;
    }

    private double CalculateAverage()
    {
        var millisecondDouble = Convert.ToDouble(OverallMilliseconds);
        var numberOfRequestsDouble = Convert.ToDouble(NumberOfRequests);
        return millisecondDouble / numberOfRequestsDouble / 1000;
    }

    private void CalculateMaximumAndMinimum(List<long> millisecondsPerRequest)
    {
        foreach (var milliseconds in millisecondsPerRequest)
        {
            var doubleMilliseconds = Convert.ToDouble(milliseconds);
            var toSeconds = doubleMilliseconds / 1000;
            if (MaximumRequestTime < toSeconds)
            {
                MaximumRequestTime = toSeconds;
            }
            else if (toSeconds < MinimumRequestTime)
            {
                MinimumRequestTime = toSeconds;
            }
        }
    }

    private double CalculateMedian()
    {
        MillisecondsPerRequest.Sort();
        var count = MillisecondsPerRequest.Count;
        var median = count % 2 == 0
            ? MillisecondsPerRequest.ElementAt(count / 2)
            : MillisecondsPerRequest.ElementAt((count - 1) / 2);

        return Convert.ToDouble(median) / 1000;
    }
}