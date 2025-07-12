using Domain.Services;

namespace Application.Services;

public class NavigationService() : INavigationService
{
    private readonly Stack<IController> _stack = new();

    public void Push(IController controller)
    {
        _stack.Push(controller);
    }

    public void Pop()
    {
        if (_stack.Count > 0) _stack.Pop();
    }

    public bool IsAtMain()
    {
        return _stack.Count == 1;
    }

    public void ReturnToMain()
    {
        var main = _stack.First();
        _stack.Clear();
        _stack.Push(main);
    }

    public void Run()
    {
        while (_stack.Count > 0)
        {
            var controller = _stack.Peek();
            controller.Run();
        }
    }
}
