using System.Data;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class CarrierController(
    INavigationService _navigationService,
    IAuthService _authService,
    ICarrierService _carrierService,
    IInputService _inputService,
    ILogger _logger) : BaseMenuController(_navigationService)
{
    public override string Title => "Cadastro de transportadoras";

    private MenuOption[] Options => [
        new ("Adicionar transportadora", Add, Role.Admin),
        new ("Editar transportadora", Edit, Role.Admin),
        new ("Remover transportadora", Remove, Role.Admin),
        new ("Listar transportadoras", List, Role.Employee),
        new ("Buscar transportadora por Id", CarrierInfoById, Role.Employee),
        new ("Buscar transportadora por nome", CarrierInfoByName, Role.Employee)
    ];

    public override IList<MenuOption> GetOptions()
    {
        return [.. Options.Where(x => _authService.DoesUserHaveAccess(x.Role, x.Mode))];
    }

    private void List()
    {
        Console.Clear();
        List<Carrier> carriers = _carrierService.GetAllCarriers();

        if (carriers.Count == 0)
        {
            LogError(_logger, "Nenhuma transportadora cadastrada.");
            return;
        }

        carriers.ForEach(Console.WriteLine);

        Console.WriteLine("\nPressione qualquer tecla para continuar.");
        Console.ReadKey();
    }

    private void Add()
    {
        Console.Clear();

        Carrier carrier;

        try
        {
            carrier = ReadCarrier();
        }
        catch (Exception exception)
        {
            LogError(_logger, "Ocorreu uma exceção.", exception);
            return;
        }

        _carrierService.AddCarrier(carrier);
        Console.Clear();
        LogSuccess(_logger, "Transportadora adicionada com sucesso.");
        PrintMenu();
    }

    private void Remove()
    {
        Console.Clear();

        if (_carrierService.GetAllCarriers().Count == 0)
        {
            ShowText("Nenhuma transportadora cadastrada.");
            return;
        }

        Carrier? carrier = SelectCarrier();

        if (carrier is null)
        {
            ShowText("Não foi possível encontrar a transportadora informada.");
            return;
        }

        _carrierService.RemoveCarrier(carrier);

        ShowText("Transportadora removida com sucesso");
    }

    private void Edit()
    {
        Console.Clear();

        if (_carrierService.GetAllCarriers().Count == 0)
        {
            ShowText("Nenhuma transportadora cadastrada.");
            return;
        }

        Carrier? carrier = SelectCarrier();

        if (carrier is null)
        {
            ShowText("Não foi possível encontrar a transportadora informada.");
            return;
        }

        carrier.Name = _inputService.ReadString("Digite o nome da transportadora: ", carrier.Name);

        if (_carrierService.GetCarriersByName(carrier.Name).Count > 0)
        {
            Console.Error.WriteLine("Já existe uma transportadora com esse nome.");
            return;
        }

        string newPrice = _inputService.ReadString("Digite o preço do transporte: ", carrier.PricePerKm.ToString());

        if (!double.TryParse(newPrice, out var price))
        {
            Console.Error.WriteLine("O preço deve ser um valor numérico.");
            return;
        }

        carrier.PricePerKm = price;

        ShowText("Transportadora atualizada com sucesso");
    }

    private void CarrierInfoByName()
    {
        Console.Clear();

        List<Carrier> carriers = _carrierService.GetAllCarriers();

        if (carriers.Count == 0)
        {
            ShowText("Nenhuma transportadora cadastrada.");
            return;
        }

        string name = _inputService.ReadString("Digite o nome da transportadora que deseja buscar ou digite 0 para voltar ao menu anterior: ");

        if (name == "0")
        {
            Console.Clear();
            return;
        }

        try
        {
            carriers = _carrierService.GetCarriersByName(name);

            if (carriers.Count > 0)
            {
                Console.Clear();
                foreach (var carrier in carriers)
                {
                    Console.WriteLine(carrier);
                }
                Console.WriteLine("\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Nenhuma transportadora encontrada com esse nome.");
            }
        }
        catch (Exception exception)
        {
            LogError(_logger, "Ocorreu um erro ao buscar a transportadora.", exception);
        }

    }

    private void CarrierInfoById()
    {
        Console.Clear();

        if (_carrierService.GetAllCarriers().Count == 0)
        {
            LogError(_logger, "Nenhuma transportadora cadastrada.");
            return;
        }

        if (!int.TryParse(_inputService.ReadString("Digite o ID da transportadora: "), out int id))
        {
            LogError(_logger, "O ID informado deve ser numérico.");
            return;
        }

        if (id == 0)
        {
            Console.Clear();
            return;
        }

        Carrier? carrier = _carrierService.GetCarrierById(id);

        if (carrier == null)
        {
            LogError(_logger, "Transportadora não encontrada.");
            return;
        }
        else
        {
            Console.Clear();
            Console.WriteLine(carrier);
            Console.WriteLine("\nPressione qualquer tecla para continuar.");
            Console.ReadKey();
        }
    }

    private Carrier? SelectCarrier()
    {
        string input = _inputService.ReadString("Digite o Id ou o nome da transportadora: ");

        if (int.TryParse(input, out var id))
        {
            return _carrierService.GetCarrierById(id);
        }
        else
        {
            return GetCarrierByName(input);
        }
    }

    private Carrier? GetCarrierByName(string name)
    {
        List<Carrier> carriers = _carrierService.GetCarriersByName(name);

        if (carriers.Count == 0)
        {
            return null;
        }

        if (carriers.Count == 1)
        {
            return carriers.Single();
        }

        Console.WriteLine("Muitas transportadoras encontradas com o mesmo nome.");
        Console.WriteLine("Informe o ID da transportadora desejada.");

        carriers.ForEach(Console.WriteLine);

        var inputId = Console.ReadLine();

        if (!int.TryParse(inputId, out var id))
        {
            LogError(_logger, "O Id informado deve ser numérico");
            return null;
        }

        return carriers.Find(x => x.Id == id);
    }

    public Carrier ReadCarrier()
    {
        Console.Clear();
        string name = _inputService.ReadString("Digite o nome da transportadora: ");

        if (_carrierService.GetCarriersByName(name).Count > 0)
        {
            throw new ArgumentException("Já existe uma transportadora com esse nome.");
        }

        if (!double.TryParse(_inputService.ReadString("Digite o valor do frete da transportadora por quilômetro: "), out double price))
        {
            throw new ArgumentException("Preço inválido.");
        }

        return new Carrier(name, price);
    }
}