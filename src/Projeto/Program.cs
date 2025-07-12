using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Infraestructure.Arrays;
using Infraestructure.List;
using Projeto.Controllers;
using Projeto.Logging;
using Projeto.Services;

namespace Projeto;

public class Program
{
    #region Repositories

    private readonly ICarrierRepo _carrierRepo;
    private readonly IClientRepo _clientRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly IProductRepo _productRepo;
    private readonly ISupplierRepo _supplierRepo;
    private readonly IUserRepo _userRepo;

    #endregion

    #region Serviços

    private readonly INavigationService _navigationService;
    private readonly ILogger _logger;

    private readonly IPersistenceService _persistenceService;
    private readonly ICarrierService _carrierService;
    private readonly IOrderService _orderService;
    private readonly IClientService _clientService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;
    private readonly IShoppingCartService _shoppingCartService;

    private readonly IValidationService _validationService;
    private readonly IInputService _inputService;

    #endregion

    public Program()
    {
        ChooseRepoType(out int option);

        if (option == 1)
        {
            _carrierRepo = new ArrayCarrierRepo();
            _clientRepo = new ArrayClientRepo();
            _orderRepo = new ArrayOrderRepo();
            _productRepo = new ArrayProductRepo();
            _supplierRepo = new ArraySupplierRepo();
            _userRepo = new ArrayUserRepo();
        }
        else
        {
            _carrierRepo = new ListCarrierRepo();
            _clientRepo = new ListClientRepo();
            _orderRepo = new ListOrderRepo();
            _productRepo = new ListProductRepo();
            _supplierRepo = new ListSupplierRepo();
            _userRepo = new ListUserRepo();

        }

        Console.Clear();
        if (InitializeRepositories())
        {
            Thread.Sleep(5000);
            Console.Clear();
        }

        _navigationService = new NavigationService();
        _logger = new ConsoleLogger();
        _validationService = new ValidationService();
        _inputService = new ConsoleInputService();
        _persistenceService = new PersistenceService();
        _carrierService = new CarrierService(_carrierRepo, _persistenceService);
        _orderService = new OrderService(_orderRepo, _persistenceService);
        _clientService = new ClientService(_clientRepo, _orderService, _persistenceService);
        _userService = new UserService(_clientService, _persistenceService, _userRepo);
        _authService = new AuthService(_userService);
        _productService = new ProductService(_productRepo, _persistenceService);
        _supplierService = new SupplierService(_supplierRepo, _productService, _persistenceService, _validationService);
        _shoppingCartService = new ShoppingCartService();

        var loginController = new LoginController(
            _navigationService,
            _authService);

        var registerController = new RegisterController(
            _navigationService,
            _userService,
            _clientService,
            _validationService,
            _inputService,
            _logger);

        var supplierController = new SupplierController(
            _navigationService,
            _authService,
            _supplierService,
            _productService,
            _inputService,
            _logger);

        var orderController = new OrderController(
            _navigationService,
            _authService,
            _orderService);

        var productController = new ProductController(
            _navigationService,
            _authService,
            _productService,
            _supplierService,
            _inputService,
            _logger);

        var carrierController = new CarrierController(
            _navigationService,
            _authService,
            _carrierService,
            _inputService,
            _logger);

        var clientController = new ClientController(
            _navigationService,
            _clientService,
            _orderService,
            _validationService,
            _inputService);

        var shoppingCartController = new ShoppingCartController(
            _navigationService,
            _authService,
            _orderService,
            _productService,
            _shoppingCartService,
            _carrierService,
            _inputService);

        var mainMenu = new MainMenuController(
            _navigationService,
            _authService,
            loginController,
            registerController,
            shoppingCartController,
            orderController,
            clientController,
            supplierController,
            productController,
            carrierController,
            _shoppingCartService);

        _navigationService.Push(mainMenu);
    }

    private static void ChooseRepoType(out int option)
    {
        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║        Escolha um modo de repositório        ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║ 1 - Vetores                                  ║");
            Console.WriteLine("║ 2 - Listas                                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.Write("\nDigite uma opção: ");
            _ = int.TryParse(Console.ReadLine(), out option);
        } while (option < 1 || option > 2);
    }

    private bool InitializeRepositories()
    {
        bool hasWarnings = false;
        PersistenceService persistence = new();

        var carriers = persistence.Load<IEnumerable<Carrier>>("transportadoras");
        var clients = persistence.Load<IEnumerable<Client>>("clientes");
        var orders = persistence.Load<IEnumerable<Order>>("pedidos");
        var products = persistence.Load<IEnumerable<Product>>("produtos");
        var suppliers = persistence.Load<IEnumerable<Supplier>>("fornecedores");
        var users = persistence.Load<IEnumerable<User>>("usuarios");

        if (carriers is null) return true;
        if (clients is null) return true;
        if (orders is null) return true;
        if (products is null) return true;
        if (suppliers is null) return true;
        if (users is null) return true;

        foreach (var carrier in carriers)
        {
            _carrierRepo.Add(carrier);
        }

        foreach (var client in clients)
        {
            _clientRepo.Add(client);
        }

        foreach (var supplier in suppliers)
        {
            _supplierRepo.Add(supplier);
        }

        foreach (var product in products)
        {
            var productSupplier = _supplierRepo.GetById(product.Supplier.Id);

            if (productSupplier is null)
            {
                Console.WriteLine($"Não foi possível encontrar o fornecedor do produto {product.Id}");
                hasWarnings = true;
                continue;
            }

            _productRepo.Add(product);
        }

        foreach (var order in orders)
        {

            var orderClient = _clientRepo.GetById(order.Client?.Id ?? -1);

            if (orderClient is null)
            {
                Console.WriteLine($"Não foi possível recuperar o cliente da ordem {order.Id}.");
                hasWarnings = true;
                continue;
            }

            var orderCarrier = _carrierRepo.GetById(order.Carrier?.Id ?? -1);

            if (orderCarrier is null)
            {
                Console.WriteLine($"Não foi possível recuperar a transportadora da ordem {order.Id}.");
                hasWarnings = true;
                continue;
            }

            order.Client = orderClient;
            order.Carrier = orderCarrier;

            List<OrderItem> invalidItems = [];

            foreach (var item in order.Items)
            {
                var itemProduct = _productRepo.GetById(item.Product.Id);

                if (itemProduct is null)
                {
                    Console.WriteLine($"Não foi possível recuperar o produto {item.Product.Id} do pedido {order.Id}.");
                    invalidItems.Add(item);
                    hasWarnings = true;
                    continue;
                }

                item.Product = itemProduct;
            }

            foreach (var item in invalidItems) order.Items.Remove(item);

            if (order.Items.Count == 0)
            {
                Console.WriteLine($"Não foi possível recuperar nenhum produto do pedido {order.Id}.");
                hasWarnings = true;
                continue;
            }

            _orderRepo.Add(order);
        }

        foreach (var user in users)
        {
            if (user.Client is not null)
            {
                var userClient = _clientRepo.GetById(user.Client.Id);

                if (userClient is not null)
                {
                    user.Client = userClient;
                }
                else
                {
                    _clientRepo.Add(user.Client);
                    user.Client = _clientRepo.GetById(user.Client.Id);
                }
            }

            _userRepo.Add(user);
        }

        return hasWarnings;
    }

    private void SaveRepositoryData()
    {
        _supplierService.Persist();
        _carrierService.Persist();
        _clientService.Persist();
        _orderService.Persist();
        _productService.Persist();
        _userService.Persist();
    }

    private void Run()
    {
        try
        {
            _navigationService.Run();
        }
        catch (Exception exception)
        {
            _logger.LogError("Ocorreu um erro durante a execução", exception);
        }

        Console.WriteLine("Salvando dados...");

        SaveRepositoryData();

        Console.Clear();
        Console.WriteLine("Programa encerrado.");
    }

    static void Main()
    {
        Program program = new();
        program.Run();
    }
}
