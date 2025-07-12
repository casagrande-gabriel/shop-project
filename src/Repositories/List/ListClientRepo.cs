using Entities;
using Repositories.Interfaces;

namespace Repositories.List;

public class ListClientRepo : IClientRepo
{
    private readonly List<Client> _clients = [];

    public IEnumerable<Client> GetAll() => _clients;

    public Client? GetById(int id) => _clients.FirstOrDefault(c => c.Id == id);

    public IEnumerable<Client> GetByName(string name) =>
        [.. _clients.Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];

    public Client? GetByCPF(string CPF)
    {
        CPF = CPF.Trim();
        CPF = CPF.Replace(".", "").Replace("-", "");
        return _clients.FirstOrDefault(c => c.CPF == CPF);
    }

    public void Add(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _clients.Add(client);
    }

    public void Remove(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _clients.Remove(client);
    }
}
