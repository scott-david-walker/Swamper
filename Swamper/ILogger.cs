namespace Swamper;

internal interface ILogger
{
    void Log(string message);
    void Log(Exception ex);
}