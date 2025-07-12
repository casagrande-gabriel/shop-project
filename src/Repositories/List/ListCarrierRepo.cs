using Entities;
using Repositories.Interfaces;

namespace Repositories.List;

public class ListCarrierRepo : ICarrierRepo
{
    private readonly List<Carrier> _carriers = [];

    public void Add(Carrier carrier)
    {
        ArgumentNullException.ThrowIfNull(carrier);
        _carriers.Add(carrier);
    }

    public void Remove(Carrier carrier)
    {
        ArgumentNullException.ThrowIfNull(carrier);
        _carriers.Remove(carrier);
    }

    public IEnumerable<Carrier> GetAll() => _carriers;

    public Carrier? GetById(int id) => _carriers.FirstOrDefault(c => c.Id == id);

    public IEnumerable<Carrier> GetByName(string name) =>
        [.. _carriers.Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];
}
