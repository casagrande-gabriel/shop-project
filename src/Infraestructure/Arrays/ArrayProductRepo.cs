using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class ArrayProductRepo : BaseArrayRepository<Product>, IProductRepo
{
    public Product? GetByName(string name)
    {
        return GetBy(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
    }
}
