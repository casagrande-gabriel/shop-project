using Domain.Entities;
using Domain.Services;
using Domain.ValueObjects;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class ClientController(
    INavigationService _navigationService,
    IClientService _clientService,
    IOrderService _orderService,
    IValidationService _validationService,
    IInputService _inputService)
    : BaseMenuController(_navigationService)
{
    public override string Title => "Cadastro de clientes";

    public override IList<MenuOption> GetOptions() => [
        new ("Adicionar cliente", AddClient),
        new ("Remover cliente", RemoveClient),
        new ("Listar clientes", WriteClients),
        new ("Detalhes do cliente", ClientDetails),
    ];

    public void AddClient()
    {
        Client? client = ReadClient();

        if (client is null) return;

        _clientService.AddClient(client);
        ShowText("Cliente cadastrado com sucesso.");
    }

    public void RemoveClient()
    {
        if (_clientService.GetAllClients().Count == 0)
        {
            ShowText("Nenhum cliente cadastradao");
            return;
        }

        while (true)
        {
            Console.Clear();

            string? input = _inputService.ReadString("Digite o nome/ID do cliente ou 0 para voltar: ");

            if (input == "0")
            {
                Console.Clear();
                return;
            }

            Client? client;

            if (!int.TryParse(input, out int id))
            {
                List<Client> clients = _clientService.GetClientsByName(input);

                if (clients.Count == 0)
                {
                    ShowText("Nenhum cliente com esse nome encontrado.");
                    continue;
                }

                client = _inputService.Choose(clients);
            }
            else
            {
                client = _clientService.GetClientById(id);
            }

            if (client is null)
            {
                ShowText("Cliente não encontrado. Tente novamente.");
                continue;
            }

            _clientService.RemoveClient(client);
            ShowText("Cliente removido com sucesso.");

            break;
        }
    }

    public void WriteClients() // mostra todos os clientes cadastrados
    {
        if (_clientService.GetAllClients().Count == 0)
        {
            ShowText("Nenhum cliente cadastrado.");
            return;
        }

        do
        {
            Console.Clear();
            Console.WriteLine("Lista de clientes cadastrados:\n");
            _clientService.GetAllClients().ForEach(Console.WriteLine);

            Console.WriteLine("\nPressione Enter para continuar.");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

        Console.Clear();
    }

    public void ClientDetails() // mostra as informações de um cliente procurando pelo ID
    {
        while (true)
        {
            Console.Clear();

            Client? client;
            Console.Write("Digite o nome/ID do cliente ou 0 para voltar: ");

            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) continue;

            if (input == "0") break;

            if (int.TryParse(input, out int id))
            {
                client = _clientService.GetClientById(id);

                if (client is null)
                {
                    ShowText("Cliente não encontrado. Tente novamente.");
                    continue;
                }
            }
            else
            {
                List<Client>? clients = _clientService.GetClientsByName(input);

                if (clients.Count == 0)
                {
                    ShowText("Cliente não encontrado. Tente novamente.");
                    continue;
                }

                if (clients.Count == 1) client = clients[0];
                else client = _inputService.Choose(_clientService.GetClientsByName(input));
            }

            if (client is null) continue;

            Console.Clear();

            Console.WriteLine($"ID: {client.Id} | Nome: {client.Name} | Telefone: {client.Phone} | E-mail: {client.Email}");
            Console.WriteLine($"Endereço: {client.Address.Street}, {client.Address.Number}, {client.Address.Complement}, " +
                $"{client.Address.Neighborhood}, {client.Address.ZipCode}, {client.Address.City}, {client.Address.State}");
            Console.WriteLine();

            var orders = _orderService.GetAllClientOrders(client.Id);

            if (orders.Count == 0)
            {
                Console.WriteLine("Cliente não possui pedidos.");
            }
            else
            {
                Console.WriteLine("Pedidos: ");
                orders.ForEach(Console.WriteLine);
            }
            Console.WriteLine();
            Console.Write("Deseja pesquisar outro cliente (S/N)? ");
            if (Console.ReadLine()?.Equals("S", StringComparison.InvariantCultureIgnoreCase) is true)
            {
                Console.Clear();
                continue;
            }

            return;
        }
    }

    public Client? ReadClient()
    {
        Console.Clear();

        string name = _inputService.ReadString("Insira o nome: ");

        if (string.IsNullOrWhiteSpace(name)) return null;

        string cpf = _inputService.ReadString("Insira seu CPF: ");

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (!_validationService.ValidateCPF(cpf))
        {
            ShowText("CPF inválido.");
            return null;
        }

        if (_clientService.GetByCPF(cpf) is not null)
        {
            ShowText("CPF já cadastrado.");
            return null;
        }

        string phone;
        string email;
        Address address;

        try
        {
            phone = _inputService.ReadString("Insira o número de telefone: ");

            if (string.IsNullOrWhiteSpace(phone)) return null;

            _validationService.ValidateTelephone(phone);

            email = _inputService.ReadString("Insira o e-mail: ");
            _validationService.ValidateEmail(email);

            address = _inputService.ReadAddress();

        }
        catch (Exception ex)
        {
            ShowText(ex.Message);
            return null;
        }

        Random random = new();
        return new(name, cpf, phone, email, address, random.NextDouble() * 80);
    }

}