using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class ArraySupplierRepo : BaseArrayRepository<Supplier>, ISupplierRepo
{
    public IEnumerable<Supplier> GetByName(string name)
    {
        return GetBy(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}
