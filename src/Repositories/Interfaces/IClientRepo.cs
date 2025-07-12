using Entities;
using Repositories.Interfaces.Base;

namespace Repositories.Interfaces;

public interface IClientRepo : IRepository<Client>
{
    IEnumerable<Client> GetByName(string name);
    Client? GetByCPF(string CPF);
}
