using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListCarrierRepo : BaseListRepository<Carrier>, ICarrierRepo
{
    public IEnumerable<Carrier> GetByName(string name) =>
        [.. _values.Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];
}
