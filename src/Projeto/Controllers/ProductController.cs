using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class ProductController(
    INavigationService _navigationService,
    IAuthService _authService,
    IProductService _productService,
    ISupplierService _supplierService,
    IInputService _inputService,
    ILogger _logger)
    : BaseMenuController(_navigationService)
{
    public override string Title => "Cadastro de produtos";

    private MenuOption[] Options => [
        new ("Adicionar produto", AddProduct, Role.Admin),
        new ("Editar produto", EditProduct, Role.Admin),
        new ("Remover produto", RemoveProduct, Role.Admin),
        new ("Listar produtos", ListProducts, Role.User),
        new ("Buscar produto por ID", ProductInfoById, Role.User),
        new ("Buscar produto por nome", ProductInfoByName, Role.User)
    ];

    public override IList<MenuOption> GetOptions()
    {
        return [.. Options.Where(x => _authService.DoesUserHaveAccess(x.Role))];
    }

    private void AddProduct()
    {
        try
        {
            Console.Clear();
            Product product = ReadProduct();

            _productService.AddProduct(product);

            ShowText("Produto adicionado com sucesso.");
        }
        catch (Exception exception)
        {
            LogError(_logger, "Não foi possível adicionar o produto.", exception);
        }
    }

    private void EditProduct()
    {
        if (_productService.GetAllProducts().Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            return;
        }

        Console.Clear();
        Product? product = SelectProduct(out string back);

        if (back == "0")
        {
            Console.Clear();
            return;
        }

        if (product is null)
        { 
            ShowText("O produto não foi encontrado"); 
            return; 
        }

        Console.Clear();
        string newName = _inputService.ReadString("Digite o novo nome do produto: ", product.Name);

        if (string.IsNullOrEmpty(newName))
        {
            Console.Error.WriteLine("O nome do produto não pode ser vazio.");
            return;
        }

        if (_productService.GetProductByName(newName) is not null && newName != product.Name)
        {
            Console.Error.WriteLine("Já existe um produto com este nome.");
            return;
        }


        string newPrice = _inputService.ReadString("Digite o novo preço do produto: ", product.Price.ToString());

        if (!double.TryParse(newPrice, out var price))
        {
            Console.Error.WriteLine("O preço deve ser um valor numérico válido.");
            return;
        }

        string newStock = _inputService.ReadString("Digite o novo estoque do produto: ", product.Stock.ToString());

        if (!uint.TryParse(newStock, out var stock))
        {
            Console.Error.WriteLine("O estoque deve ser um valor numérico válido.");
            return;
        }

        List<Supplier> suppliers = _supplierService.GetAllSuppliers();

        Console.Clear();

        suppliers.ForEach(Console.WriteLine);

        if (!int.TryParse(_inputService.ReadString("\nDigite o ID do novo fornecedor: "), out int supplierId))
        {
            Console.Clear();
            Console.Error.WriteLine("ID inválido. Deve ser um número positivo.");
            Thread.Sleep(1500);
            Console.Clear();
            return;
        }

        Supplier? supplier = _supplierService.GetSupplierById(supplierId);

        if (supplier is null)
        {
            Console.Clear();
            Console.Error.WriteLine("Fornecedor não encontrado.");
            Thread.Sleep(1500);
            Console.Clear();
            return;
        }

        product.Name = newName;
        product.Price = price;
        product.Stock = stock;

        product.Supplier = supplier;

        ShowText("Produto atualizado com sucesso");
    }

    private void RemoveProduct()
    {
        if (_productService.GetAllProducts().Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            return;
        }

        Product? product = SelectProduct(out string back);

        if (back == "0")
        {
            Console.Clear();
            return;
        }

        if (product is null)
        {
            ShowText("O produto não foi encontrado");
            return;
        }

        _productService.RemoveProduct(product);

        ShowText("Produto removido com sucesso.");
    }

    private void ListProducts()
    {
        List<Product> products = _productService.GetAllProducts();

        if (products.Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            return;
        }

        Console.Clear();
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private Product? SelectProduct(out string back)
    {
        List<Product> products = _productService.GetAllProducts();

        if (products.Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            back = "0";
            return null;
        }

        Product? product;

        Console.Clear();
        products.ForEach(Console.WriteLine);

        string input = _inputService.ReadString("\nDigite o nome/ID do produto ou 0 para voltar: ");

        if (input == "0")
        {
            Console.Clear();
            back = "0";
            return null;
        }

        if (string.IsNullOrEmpty(input))
        {
            ShowText("Nenhum produto informado.");
            back = "0";
            return null;
        }

        back = input;

        if (int.TryParse(input, out var id))
        {
            product = _productService.GetProductById(id);
        }
        else
        {
            product = _productService.GetProductByName(input);
        }

        return product;
    }

    private void ProductInfoById()
    {
        Console.Clear();

        if (_productService.GetAllProducts().Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            return;
        }

        string input = _inputService.ReadString("Digite o ID do produto ou digite 0 para voltar ao menu anterior: ");

        if (input == "0")
        {
            Console.Clear();
            return;
        }

        if (!int.TryParse(input, out int id))
        {
            ShowText("ID inválido. Deve ser um número.");
            return;
        }

        if (id <= 0)
        {
            ShowText("ID inválido. Deve ser um número positivo.");
            return;
        }

        Product? product = _productService.GetProductById(id);

        if (product is null)
        {
            ShowText("Produto não encontrado.");
            return;
        }

        Console.Clear();
        Console.WriteLine(product);
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    private void ProductInfoByName()
    {
        Console.Clear();

        if (_productService.GetAllProducts().Count == 0)
        {
            ShowText("Nenhum produto cadastrado.");
            return;
        }

        string name = _inputService.ReadString("Digite o nome do produto ou digite 0 para voltar ao menu anterior: ");

        if (name == "0")
        {
            Console.Clear();
            return;
        }

        Product? product = _productService.GetProductByName(name);

        if (product is null)
        {
            ShowText("Nenhum produto encontrado com esse nome.");
            return;
        }

        Console.Clear();
        Console.WriteLine(product);

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public Product ReadProduct()
    {
        List<Supplier>? suppliers = _supplierService.GetAllSuppliers();

        if (suppliers.Count == 0) throw new Exception("Cadastre um fornecedor primeiro.");

        string name = _inputService.ReadString("Insira o nome do produto: ");

        if (string.IsNullOrEmpty(name)) throw new ArgumentException("O nome deve ser informado.");

        if (_productService.GetProductByName(name) is not null)
        {
            throw new ArgumentException("Já existe um produto com este nome.");
        }

        if (!double.TryParse(_inputService.ReadString("Insira o preço do produto: "), out double price) || price <= 0)
        {
            throw new ArgumentException("Preço inválido.");
        }

        if (!uint.TryParse(_inputService.ReadString("Insira a quantidade em estoque do produto: "), out uint stock) || stock <= 0)
        {
            throw new ArgumentException("Quantidade inválida.");
        }

        Console.Clear();

        suppliers.ForEach(Console.WriteLine);

        if (!int.TryParse(_inputService.ReadString("\nDigite o ID do fornecedor: "), out int supplierId))
        {
            throw new ArgumentException("ID inválido.");
        }

        Product? product;
        Supplier supplier = _supplierService.GetSupplierById(supplierId) ?? throw new Exception("Fornecedor não encontrado.");

        product = new(name, price, stock, supplier);

        return product;
    }
}