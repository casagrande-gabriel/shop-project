using Domain.Entities;

namespace Domain.Services;

public interface IProductService
{
    void AddProduct(Product product);
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    Product? GetProductByName(string name);
    List<Product> GetAllProductsOfSupplier(Supplier supplier);
    void RemoveProduct(Product product);
    void Persist();
}