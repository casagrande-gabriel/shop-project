using Entities;
using Repositories.Interfaces.Base;

namespace Repositories.Interfaces;

public interface ISupplierRepo : IRepository<Supplier>
{
    IEnumerable<Supplier> GetByName(string name);
}
