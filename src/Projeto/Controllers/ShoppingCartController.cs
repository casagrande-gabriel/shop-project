using Domain.Entities;
using Domain.Services;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class ShoppingCartController(
    INavigationService _nav,
    IAuthService _authService,
    IOrderService _orderService,
    IProductService _productService,
    IShoppingCartService _cartService,
    ICarrierService _carrierService,
    IInputService inputService)
    : BaseMenuController(_nav)
{
    private Client? client;

    public override void Run()
    {
        client = _authService.Client!;
        base.Run();
    }

    public override string Title => "Carrinho de compras";

    public override IList<MenuOption> GetOptions() => [
        new ("Adicionar produto ao carrinho", AddProductToCart),
        new ("Remover produto do carrinho", RemoveProductFromCart),
        new ("Listar produtos no carrinho", ListProductsInCart),
        new ("Esvaziar carrinho", ClearCart),
        new ("Finalizar compra", Checkout)
    ];

    private Carrier? carrier = RandomizeCarrier(_carrierService.GetAllCarriers());
    private Order? order;
    private static readonly Random random = new();

    public void AddProductToCart()
    {
        Console.Clear();

        Console.WriteLine("Lista de produtos disponíveis:");
        List<Product> products = [.. _productService.GetAllProducts().Where(p => p.Stock > 0)];
        products.ForEach(Console.WriteLine);

        string input = inputService.ReadString("\nDigite o nome/ID ou digite 0 para voltar: ");

        if (input == "0")
        {
            Console.Clear();
            return;
        }

        Product? product;

        if (int.TryParse(input, out int productId))
        {
            product = _productService.GetProductById(productId);
        }
        else product = _productService.GetProductByName(input);

        if (product == null)
        {
            ShowText("Nome ou ID inválido.");
            return;
        }

        input = inputService.ReadString("\nDigite a quantidade: ");

        if (input == "0")
        {
            Console.Clear();
            return;
        }

        if (!uint.TryParse(input, out uint quantity) || quantity <= 0)
        {
            ShowText("Quantidade inválida.");
            return;
        }

        if (quantity > product.Stock)
        {
            ShowText("Não há produtos suficientes em estoque.");
            return;
        }

        _cartService.AddQuantity(product, quantity);
        ShowText("Produto adicionado ao carrinho.");
    }

    public void RemoveProductFromCart()
    {
        if (_cartService.IsEmpty())
        {
            ShowText("Carrinho vazio.");
            return;
        }

        Console.Clear();
        _cartService.Print();

        string? input = inputService.ReadString("\nDigite o nome/ID do produto ou digite 0 para voltar: ");

        if (input == "0")
        {
            Console.Clear();
            return;
        }

        Product? product;

        if (int.TryParse(input, out int id))
        {
            product = _productService.GetProductById(id);
        }
        else product = _productService.GetProductByName(input);

        if (product is null)
        {
            ShowText("Nome ou ID inválido.");
            return;
        }

        input = inputService.ReadString("\nDigite a quantidade a ser removida: ");

        if (input == "0")
        {
            Console.Clear();
            return;
        }

        if (!uint.TryParse(input, out uint quantity) || quantity <= 0)
        {
            ShowText("Quantidade inválida.");
            return;
        }

        if (quantity >= _cartService.GetQuantity(product))
        {
            _cartService.Remove(product);
            ShowText("Produto removido do carrinho.");
            return;
        }

        _cartService.RemoveQuantity(product, quantity);
        ShowText("Quantidade removida do carrinho.");
    }

    public void ListProductsInCart()
    {
        if (_cartService.IsEmpty())
        {
            ShowText("Carrinho vazio.");
            return;
        }

        do
        {
            Console.Clear();

            Console.WriteLine("Produtos no carrinho:\n");
            _cartService.Print();

            Console.WriteLine("\nPressione Enter para continuar.");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

        Console.Clear();
    }

    public void ClearCart()
    {
        if (_cartService.IsEmpty())
        {
            ShowText("Carrinho já está vazio.");
            return;
        }

        _cartService.Clear();
        ShowText("Carrinho esvaziado.");
    }

    public void Checkout()
    {
        if (_cartService.IsEmpty())
        {
            ShowText("Carrinho vazio. Adicione produtos para finalizar a compra.");
            return;
        }

        if (carrier is null)
        {
            carrier = RandomizeCarrier(_carrierService.GetAllCarriers());

            if (carrier is null)
            {
                ShowText("Nenhuma transportadora disponível para entrega.");
                return;
            }
        }

        Console.Clear();
        Console.WriteLine("Produtos no carrinho:");

        _cartService.Print();
        double sum = _cartService.GetTotalPrice();

        Console.WriteLine($"\nTotal: {sum:C}");

        double deliveryFee = Math.Log10(client!.Distance) * carrier.PricePerKm;

        Console.WriteLine($"Entrega: {deliveryFee:C}");
        sum += deliveryFee;

        Console.WriteLine($"Subtotal: {sum:C}\n");

        Console.WriteLine("Deseja finalizar a compra? (S/N)");
        string? input = Console.ReadLine()?.Trim().ToUpperInvariant();

        if (input == "N")
        {
            Console.Clear();
            return;
        }

        if (input != "S")
        {
            ShowText("Opção inválida. Tente novamente.");
            return;
        }

        order = new(_authService.Client!, carrier, deliveryFee);

        foreach (var (product, quantity) in _cartService.GetCart())
        {
            _orderService.AddItemToOrder(order, product, quantity);
        }

        _orderService.AddOrder(order);
        _cartService.ConcludeSale();
        carrier = RandomizeCarrier(_carrierService.GetAllCarriers());
        ShowText("Pedido realizado com sucesso!");
    }

    private static Carrier RandomizeCarrier(List<Carrier> carriers)
    {
        if (carriers.Count == 0) return null!;
        return carriers[random.Next(carriers.Count)];
    }
}