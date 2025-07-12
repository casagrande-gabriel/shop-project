using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepo : IRepository<Product>
{
    Product? GetByName(string name);
}
