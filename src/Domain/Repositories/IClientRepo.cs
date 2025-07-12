using Domain.Entities;

namespace Domain.Repositories;

public interface IClientRepo : IRepository<Client>
{
    IEnumerable<Client> GetByName(string name);
    Client? GetByCPF(string CPF);
}
