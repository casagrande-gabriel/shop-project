using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListSupplierRepo : BaseListRepository<Supplier>, ISupplierRepo
{
    public IEnumerable<Supplier> GetByName(string name) =>
        [.. _values.Where(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];
}