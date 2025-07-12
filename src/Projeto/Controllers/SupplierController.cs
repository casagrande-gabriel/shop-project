using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Domain.ValueObjects;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class SupplierController(
    INavigationService _navigationService,
    IAuthService _authService,
    ISupplierService _supplierService,
    IProductService _productService,
    IInputService _inputService,
    ILogger _logger)
    : BaseMenuController(_navigationService)
{
    public override string Title => "Cadastro de fornecedores";

    private MenuOption[] Options => [
        new ("Adicionar fornecedor", Add, Role.Admin),
        new ("Editar fornecedor", Edit, Role.Admin),
        new ("Remover fornecedor", Remove, Role.Admin),
        new ("Listar fornecedores", ListSuppliers, Role.Employee),
        new ("Listar produtos", ListProducts, Role.Employee)
    ];

    public override MenuOption[] GetOptions()
    {
        return [.. Options.Where(x => _authService.DoesUserHaveAccess(x.Role, x.Mode))];
    }

    private void ListProducts()
    {
        Console.Clear();
        Supplier? supplier = SelectSupplier(out string back);

        if (back == "0")
        {
            Console.Clear();
            return;
        }

        if (supplier is null)
        {
            ShowText("Fornecedor não encontrado.");
            return;
        }

        var products = _productService.GetAllProductsOfSupplier(supplier);

        if (products.Count == 0)
        {
            ShowText("Nenhum produto cadastrado para este fornecedor.");
            return;
        }

        do
        {
            Console.Clear();
            Console.WriteLine($"Produtos do fornecedor {supplier.Name}: ");
            products.ForEach(Console.WriteLine);

            Console.WriteLine("\nPressione enter para continuar.");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

        Console.Clear();
    }

    private void ListSuppliers()
    {
        List<Supplier> suppliers = _supplierService.GetAllSuppliers();

        if (suppliers.Count == 0)
        {
            ShowText("Nenhum fornecedor cadastrado.");
            return;
        }

        do
        {
            Console.Clear();
            Console.WriteLine("Fornecedores: ");
            suppliers.ForEach(Console.WriteLine);

            Console.WriteLine("\nPressione Enter para continuar.");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

        Console.Clear();
    }

    private void Add()
    {
        Console.Clear();
        try
        {
            var supplier = ReadSupplier();
            _supplierService.AddSupplier(supplier);
            LogSuccess(_logger, "Fornecedor adicionado com sucesso.");
        }
        catch (Exception exception)
        {
            LogError(_logger, "Ocorreu um erro ao adicionar o fornecedor", exception);
        }
    }

    private void Remove()
    {
        Supplier? supplier = SelectSupplier(out string back);

        if (back == "0")
        {
            Console.Clear();
            return;
        }

        if (supplier is null)
        {
            ShowText("Não foi encontrado nenhum fornecedor.");
            return;
        }

        _supplierService.DeleteSupplier(supplier);

        ShowText("Fornecedor excluído com sucesso.");
    }

    private void Edit()
    {
        Supplier? supplier = SelectSupplier(out string back);

        if (back == "0")
        {
            Console.Clear();
            return;
        }

        if (supplier is null)
        {
            ShowText("Não foi encontrado nenhum fornecedor.");
            return;
        }

        Console.Clear();

        string? newName = _inputService.ReadString("Digite o nome do fornecedor: ", supplier.Name);
        string? newDesc = _inputService.ReadString("Digite a descrição do fornecedor: ", supplier.Description ?? string.Empty);
        string? newEmail = _inputService.ReadString("Digite o e-mail do fornecedor: ", supplier.Email);
        string? newPhone = _inputService.ReadString("Digite o telefone do fornecedor: ", supplier.PhoneNumber);
        Address newAddress = _inputService.ReadAddress(supplier.Address);

        Supplier temp = new(newName, newDesc, newPhone, newEmail, newAddress);

        try
        {
            _supplierService.ValidateSupplier(temp);

            supplier.Name = newName;
            supplier.Description = newDesc;
            supplier.Email = newEmail;
            supplier.PhoneNumber = newPhone;
            supplier.Address = newAddress;
        }
        catch (Exception exception)
        {
            LogError(_logger, "Não foi possível atualizar o fornecedor. ", exception);
            return;
        }

        ShowText("Fornecedor atualizado com sucesso.");
    }

    private Supplier? SelectSupplier(out string back)
    {
        List<Supplier> suppliers = _supplierService.GetAllSuppliers();

        if (suppliers.Count == 0)
        {
            ShowText("Nenhum fornecedor cadastrado.");
            back = "0";
            return null;
        }

        Console.Clear();
        suppliers.ForEach(Console.WriteLine);

        var input = _inputService.ReadString("\nDigite o nome/ID do fornecedor ou digite 0 para voltar: ");

        if (input == "0")
        {
            back = "0";
            return null;
        }

        back = input;

        if (int.TryParse(input, out var supplierId))
        {
            return _supplierService.GetSupplierById(supplierId);
        }
        else
        {
            return GetSupplierByName(input);
        }
    }

    private Supplier? GetSupplierByName(string name)
    {
        List<Supplier> suppliers = _supplierService.GetSuppliersByName(name);

        if (suppliers.Count == 0)
        {
            return null;
        }

        if (suppliers.Count == 1)
        {
            return suppliers.Single();
        }

        Console.Clear();
        Console.WriteLine("Muitos fornecedores encontrados com o mesmo nome.");
        Thread.Sleep(1500);

        Console.WriteLine("Informe o ID do fornecedor desejado:");

        suppliers.ForEach(Console.WriteLine);

        string? inputId = Console.ReadLine();

        if (!int.TryParse(inputId, out var id))
        {
            LogError(_logger, "O ID informado deve ser numérico");
            return null;
        }

        return suppliers.Find(x => x.Id == id);
    }

    public Supplier ReadSupplier()
    {
        Console.WriteLine("Digite os dados do novo fornecedor\n");

        string name = _inputService.ReadString("Insira o nome do fornecedor: ");
        string description = _inputService.ReadString("Insira a descrição do fornecedor: ");
        string phoneNumber = _inputService.ReadString("Insira o telefone do fornecedor: ");
        string email = _inputService.ReadString("Insira o e-mail do fornecedor: ");

        Console.WriteLine("\nInsira os dados de endereço do fornecedor");
        Address address = _inputService.ReadAddress();

        return new Supplier(name, description, phoneNumber, email, address);
    }
}