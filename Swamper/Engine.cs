using System.Collections.Concurrent;
using System.Diagnostics;
namespace Swamper;

internal class Engine
{
    private readonly int _threads;
    private readonly CancellationToken _token;
    private readonly HttpClientConnection _clientConnection;
    private readonly Stopwatch _sw = new();
    private readonly ConcurrentQueue<JobResult> _queue = new();

    internal Engine(int threads, HttpClientConnection clientConnection,  CancellationToken token)
    {
        _threads = threads;
        _clientConnection = clientConnection;
        _token = token;
    }
    
    private readonly ILogger _logger = new ConsoleLogger();
    private const int MaxWaitHandles = 64; //Maximum of library
    internal Task<Result> Run()
    {
        return Task.Run(Start, _token);
    }

    private Result Start()
    {
        _sw.Start();
        var events = TriggerThreads();
        WaitForThreadCompletion(events);
        return new Result().ParseAllResults(_queue);
    }

    private static void WaitForThreadCompletion(IReadOnlyCollection<ManualResetEventSlim> events)
    {
        for (var i = 0; i < events.Count; i += MaxWaitHandles)
        {
            var handles = events.Skip(i).Take(MaxWaitHandles).Select(r => r.WaitHandle).ToArray();
            WaitHandle.WaitAll(handles);
        }
    }

    private List<ManualResetEventSlim> TriggerThreads()
    {
        var events = new List<ManualResetEventSlim>();
        for (var i = 0; i < _threads; i++)
        {
            var resetEvent = new ManualResetEventSlim(false);
            var thread = new Thread(async () => await Work(resetEvent));
            thread.Start();
            events.Add(resetEvent);
        }

        return events;
    }

    private async Task Work(ManualResetEventSlim resetEvent)
    {
        HttpClientConnection job;
        try
        {
            job = await _clientConnection.Initialise();
        }
        catch (Exception)
        {
            resetEvent.Set();
            return;
        }

        await RunUntilTimerEnd(resetEvent, job);
    }

    private async Task RunUntilTimerEnd(ManualResetEventSlim resetEvent, HttpClientConnection job)
    {
        
        while (!_token.IsCancellationRequested)
        {
            try
            {
                await job.Connect();
                
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        _queue.Enqueue(job.GetResults());
        resetEvent.Set();
    }
}

internal class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void Log(Exception ex)
    {
        Console.WriteLine(ex);
    }
}