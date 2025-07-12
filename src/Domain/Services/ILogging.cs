namespace Domain.Services;

public interface ILogger
{
    public void Log(string message);
    public void LogError(Exception exception);
    public void LogError(string errorMessage, Exception? exception = null);
}