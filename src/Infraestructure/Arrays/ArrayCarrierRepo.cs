using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class ArrayCarrierRepo : BaseArrayRepository<Carrier>, ICarrierRepo
{
    public IEnumerable<Carrier> GetByName(string name)
    {
        return GetBy(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}
