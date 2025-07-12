using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListProductRepo : BaseListRepository<Product>, IProductRepo
{
    public Product? GetByName(string name) =>
        _values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
}
