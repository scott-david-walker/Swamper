namespace Swamper;

public class ConsolePresenter : IPresenter
{
    public void Present(Result result)
    {
        Console.WriteLine($"Number of requests: {result.NumberOfRequests}");
        Console.WriteLine($"Average time per request in seconds: {result.AverageTimePerRequest}");
        Console.WriteLine($"Maximum request time: {result.MaximumRequestTime}");
        Console.WriteLine($"Minimum request time: {result.MinimumRequestTime}");
        Console.WriteLine($"Median request time: {result.MedianRequestTime}");
        Console.WriteLine("Returned Status Codes:");
        foreach (var statusCodes in result.StatusCodes)
        {
            Console.Write("    "); //indent
            Console.WriteLine($"{statusCodes.Key} : {statusCodes.Value}");
        }
    }
}