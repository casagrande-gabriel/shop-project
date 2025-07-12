using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListClientRepo : BaseListRepository<Client>, IClientRepo
{
    public IEnumerable<Client> GetByName(string name) =>
        [.. _values.Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];

    public Client? GetByCPF(string CPF)
    {
        CPF = CPF.Trim();
        CPF = CPF.Replace(".", "").Replace("-", "");
        return _values.FirstOrDefault(c => c.CPF == CPF);
    }
}
