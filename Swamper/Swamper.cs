namespace Swamper;

public static class Swamper
{
    public static async Task Swamp()
    {
        var threads = 100;
        var lengthOfTime = 60;//seconds
        await new Engine(threads, TimeSpan.FromSeconds(lengthOfTime)).Run();
    }
}