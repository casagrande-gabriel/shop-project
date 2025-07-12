using Domain.Services;

namespace Projeto.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(1500);
        Console.Clear();
    }

    public void LogError(string errorMessage, Exception? exception = null)
    {
        Console.Clear();
        Console.Error.WriteLine("{0} {1}", errorMessage, exception?.Message);
        Thread.Sleep(1500);
        Console.Clear();
    }

    public void LogError(Exception exception)
    {
        LogError("Uma exceção ocorreu", exception);
    }
}