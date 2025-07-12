using Domain.Entities;

namespace Domain.Repositories;

public interface ICarrierRepo : IRepository<Carrier>
{
    IEnumerable<Carrier> GetByName(string name);
}
