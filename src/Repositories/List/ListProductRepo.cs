using Entities;
using Repositories.Interfaces;

namespace Repositories.List;

public class ListProductRepo : IProductRepo
{
    private readonly List<Product> _products = [];

    public IEnumerable<Product> GetAll() => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public Product? GetByName(string name) => 
        _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _products.Add(product);
    }

    public void Remove(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _products.Remove(product);
    }
}
