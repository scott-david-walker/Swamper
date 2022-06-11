namespace Swamper;

public static class Swamper
{
    public static async Task Swamp()
    {
        var threads = 100;
        var lengthOfTime = 6;//seconds
        var result = await new Engine(threads, TimeSpan.FromSeconds(lengthOfTime)).Run();
        new ConsolePresenter().Present(result);
    }
}