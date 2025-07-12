using Entities;
using Repositories.Interfaces;

namespace Repositories.Arrays;

public class ArrayClientRepo : IClientRepo
{
    private Client[] _clients = new Client[10];

    int nextIndex = 0;

    private void ArrayResize()
    {
        Client[] temp = _clients;
        _clients = new Client[_clients.Length * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _clients[i] = temp[i];
        }
    }

    public void Add(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _clients[nextIndex] = client;

        nextIndex++;
        if (nextIndex >= _clients.Length)
        {
            ArrayResize();
        }
    }

    public IEnumerable<Client> GetAll()
    {
        if (nextIndex == 0) return [];

        Client[] result = new Client[nextIndex];
        for (int i = 0; i < nextIndex; i++)
        {
            result[i] = _clients[i];
        }

        return result;
    }

    public Client? GetById(int id)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_clients[i].Id == id) return _clients[i];
        }

        return null;
    }

    public IEnumerable<Client> GetByName(string name)
    {
        if (nextIndex == 0) return [];

        int i;
        int counter = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_clients[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) counter++;
        }

        Client[] clients = new Client[counter];

        int j = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_clients[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                clients[j++] = _clients[i];
            }
        }

        return counter > 0 ? clients : [];
    }

    public Client? GetByCPF(string CPF)
    {
        if (nextIndex == 0) return null;

        CPF = CPF.Trim();
        CPF = CPF.Replace(".", "").Replace("-", "");

        foreach (Client client in _clients)
        {
            if (client is not null && client.CPF == CPF) return client;
        }

        return null;
    }

    public void Remove(Client client)
    {
        int index = -1;
        int i;

        for (i = 0; i < nextIndex; i++)
        {
            if (_clients[i] == client)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Cliente não encontrado.");
            return;
        }

        for (; i < nextIndex - 1; i++)
        {
            _clients[i] = _clients[i + 1];
        }

        _clients[nextIndex - 1] = null!;

        nextIndex--;
    }
}