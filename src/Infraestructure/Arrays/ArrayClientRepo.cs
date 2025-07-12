using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class ArrayClientRepo : BaseArrayRepository<Client>, IClientRepo
{
    public IEnumerable<Client> GetByName(string name)
    {
        return GetBy(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public Client? GetByCPF(string cpf)
    {
        if (_index == 0) return null;

        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");

        return GetBy(x => x.CPF.Equals(cpf)).FirstOrDefault();
    }
}