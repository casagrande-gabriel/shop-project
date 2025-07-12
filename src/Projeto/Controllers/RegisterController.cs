using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.ValueObjects;
using Projeto.Controllers.Base;

namespace Projeto.Controllers;

public class RegisterController(
    INavigationService _navigationService,
    IUserService _userService,
    IClientService _clientService,
    IValidationService _validationService,
    IInputService inputService,
    ILogger _logger)
    : BaseController(_navigationService)
{
    public override void Run()
    {
        Console.Clear();

        Console.WriteLine("Digite os dados ou 0 a qualquer momento para voltar\n");

        Console.Write("Nome de usuário: ");
        string? username = Console.ReadLine();

        if (username == "0")
        {
            GoBack();
            return;
        }

        if (_userService.UserExists(username!))
        {
            Console.Clear();
            Console.WriteLine("Usuário já existe.");
            Thread.Sleep(1500);
            return;
        }

        Console.Write("Digite a senha: ");
        string? password = Console.ReadLine();

        if (password == "0")
        {
            GoBack();
            return;
        }

        if (!IsValidPassword(password!))
        {
            Console.Clear();
            Console.WriteLine("A senha precisa ter no mínimo 8 caracteres.");
            Thread.Sleep(1500);
            return;
        }

        Console.Clear();


        do
        {

            try
            {
                string? name = inputService.ReadString("Digite seu nome completo: ");

                if (string.IsNullOrWhiteSpace(name)) throw new Exception("Nome completo é obrigatório.");

                string? CPF = inputService.ReadString("Digite seu CPF: ");

                if (!_validationService.ValidateCPF(CPF)) throw new Exception("CPF inválido. Tente novamente.");

                CPF = CPF.Trim();
                CPF = CPF.Replace(".", "").Replace("-", "");

                Client? client = _clientService.GetByCPF(CPF);

                string? email = inputService.ReadString("Digite seu e-mail: ");
                _validationService.ValidateEmail(email!);


                string? phone = inputService.ReadString("Digite seu telefone: ");
                _validationService.ValidateTelephone(phone!);

                Address address = inputService.ReadAddress();
                _validationService.ValidateAddress(address);

                Random random = new();

                client = new(name, CPF, email, phone, address, random.NextDouble() * 800);
                _clientService.AddClient(client);
                _userService.Register(client , username!, password!, Role.User);

                break;
            }
            catch (Exception exception)
            {
                Console.Clear();
                LogError(_logger, "Erro ao cadastrar cliente: ", exception);
                Thread.Sleep(1500);
                continue;
            }

        } while (true);

        Console.WriteLine("Usuário cadastrado com sucesso.");
        Thread.Sleep(1500);

        GoBack();
    }

    private static bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
    }
}
