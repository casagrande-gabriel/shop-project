using Domain.Entities;

namespace Domain.Repositories;

public interface ISupplierRepo : IRepository<Supplier>
{
    IEnumerable<Supplier> GetByName(string name);
}
