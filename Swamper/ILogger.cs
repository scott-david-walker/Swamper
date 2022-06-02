namespace Swamper;

public interface ILogger
{
    void Log(string message);
    void Log(Exception ex);
}