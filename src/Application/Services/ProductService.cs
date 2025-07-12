using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class ProductService (
    IProductRepo _productRepo,
    IPersistenceService _persistenceService) : IProductService
{
    public void AddProduct(Product product)
    {
        if (product is null) throw new ArgumentNullException(nameof(product), "Produto nulo");
        if (_productRepo.GetByName(product.Name) is not null)
        {
            Console.WriteLine("Já existe um produto com este nome.");
            return;
        }
        _productRepo.Add(product);
        Persist();
    }

    public void RemoveProduct(Product product)
    {
        _productRepo.Remove(product);
        Persist();
    }

    public List<Product> GetAllProducts() => [.. _productRepo.GetAll()];
    public List<Product> GetAllProductsOfSupplier(Supplier supplier) => _productRepo.GetAll().Where(x => x.Supplier.Id == supplier.Id).ToList();
    public Product? GetProductById(int id) => _productRepo.GetById(id);
    public Product? GetProductByName(string name) => _productRepo.GetByName(name);
    public void Persist() => _persistenceService.Save("produtos", _productRepo.GetAll());
}
