using System.Text;
using Domain.Services;
using Projeto.Controllers.Models;

namespace Projeto.Controllers.Base;

public abstract class BaseMenuController : BaseController
{
    public BaseMenuController(INavigationService navigationService)
        : base(navigationService)
    {
    }

    public abstract string Title { get; }
    public abstract IList<MenuOption> GetOptions();

    public override void Run()
    {
        var options = GetOptions();

        Console.Clear();
        PrintMenu();

        Console.Write("Digite uma opção: ");
        string? line = Console.ReadLine();

        if (string.IsNullOrEmpty(line))
        {
            return;
        }

        if (!int.TryParse(line, out var option))
        {
            Console.Error.WriteLine("A entrada deve ser um valor numérico.");
            return;
        }

        if (option < 0 || option > options.Count)
        {
            Console.Error.WriteLine("A entrada não corresponde a nenhuma ação.");
            return;
        }

        if (option == 0)
        {
            GoBack();
            return;
        }

        options[option - 1].Action.Invoke();
    }

    protected virtual void PrintMenu()
    {
        int index = 1;
        var options = GetOptions();

        Console.WriteLine("╔══════════════════════════════════════════════╗");

        PrintLine(Title);

        Console.WriteLine("╠══════════════════════════════════════════════╣");

        foreach (var option in options)
        {
            PrintLine($"{index++} - {option}");
        }

        PrintLine();

        if (_navigationService.IsAtMain())
        {
            PrintLine("0 - Encerrar programa");
        }
        else
        {
            PrintLine("0 - Voltar");
        }

        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }

    protected virtual void PrintLine(string? content = null)
    {
        const int COLUMNS = 48;

        StringBuilder builder = new(COLUMNS);
        builder.Append("║ ");

        if (content is not null)
        {
            builder.Append(content);
        }

        builder.Append(' ', builder.Capacity - builder.Length - 2 /* por conta do final da linha */);
        builder.Append(" ║");

        Console.WriteLine(builder.ToString());
    }

    protected virtual void ShowText(string text)
    {
        Console.Clear();
        Console.WriteLine(text);
        Thread.Sleep(1500);
        Console.Clear();
    }
}