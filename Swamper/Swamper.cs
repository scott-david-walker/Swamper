﻿namespace Swamper;

public static class Swamper
{
    public static async Task Swamp()
    {
        await Swamp(new Uri("https://localhost:7276/weatherforecast"), 100, 60);
    }
    public static async Task Swamp(Uri url, int threads, int lengthOfTimeInSeconds)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, url);
        await Swamp(message, threads, lengthOfTimeInSeconds);
    }
    public static async Task Swamp(HttpRequestMessage message, int threads, int lengthOfTimeInSeconds)
    {
        await Swamp(message, threads, lengthOfTimeInSeconds, new ConsolePresenter());
    }
    public static async Task Swamp(HttpRequestMessage message, int threads, int lengthOfTimeInSeconds, IPresenter presenter)
    {
        var result = await new Engine(threads, TimeSpan.FromSeconds(lengthOfTimeInSeconds), new HttpClientConnection(message, new HttpClient())).Run();
        presenter.Present(result);
    }
}