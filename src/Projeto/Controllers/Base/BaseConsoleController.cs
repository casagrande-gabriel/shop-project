using Domain.Services;

namespace Projeto.Controllers.Base;

public abstract class BaseController : IController
{
    protected readonly INavigationService _navigationService;

    public BaseController(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected void GoTo(IController controller) => _navigationService.Push(controller);
    protected void GoBack() => _navigationService.Pop();
    protected static void LogSuccess(ILogger logger, string message)
    {
        logger.Log(message);
    }

    protected static void LogError(ILogger logger, string message, Exception? exception = null)
    {
        logger.LogError(message, exception);
    }

    public abstract void Run();
}