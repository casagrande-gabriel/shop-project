using Entities;
using Repositories.Interfaces.Base;

namespace Repositories.Interfaces;

public interface ICarrierRepo : IRepository<Carrier>
{
    IEnumerable<Carrier> GetByName(string name);
}
