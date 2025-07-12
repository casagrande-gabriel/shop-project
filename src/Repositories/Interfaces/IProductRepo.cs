using Entities;
using Repositories.Interfaces.Base;

namespace Repositories.Interfaces;

public interface IProductRepo : IRepository<Product>
{
    Product? GetByName(string name);
}
