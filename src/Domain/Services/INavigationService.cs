namespace Domain.Services;

public interface INavigationService
{
    bool IsAtMain();
    void Pop();
    void Push(IController controller);
    void ReturnToMain();
    void Run();
}