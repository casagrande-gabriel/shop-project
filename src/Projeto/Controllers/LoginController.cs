using Domain.Services;
using Projeto.Controllers.Base;

namespace Projeto.Controllers;

public class LoginController(
    INavigationService _navigationService,
    IAuthService _authService)
    : BaseController(_navigationService)
{

    public override void Run()
    {
        Console.Clear();

        Console.WriteLine("Digite os dados ou 0 a qualquer momento para voltar\n");

        if (!LoginMenu(out string? username, out string? password))
        {
            Console.WriteLine("Entrada inválida. Tente novamente.");
            Thread.Sleep(1500);
            Console.Clear();
            return;
        }

        if (username == "0" || password == "0")
        {
            GoBack();
            return;
        }

        if (!_authService.Login(username!, password!))
        {
            Console.WriteLine("Usuário ou senha inválidos.");
            Thread.Sleep(1500);
            Console.Clear();
            return;
        }

        Console.Clear();

        if (_authService.IsAdmin)
        {
            Console.WriteLine("Login realizado como administrador.");
        }
        else
        {
            Console.WriteLine("Login realizado com sucesso.");
        }

        Thread.Sleep(1500);
        Console.Clear();

        GoBack();
    }
    
    private static bool LoginMenu(out string? username, out string? password)
    {
        Console.Write("Usuário: ");
        username = Console.ReadLine();

        if (username == "0")
        {
            password = null;
            return true;
        }

        Console.Write("Senha: ");
        password = Console.ReadLine();
        Console.Clear();

        if (password == "0") return true;
        
        return username != null && password != null;
    }
}
