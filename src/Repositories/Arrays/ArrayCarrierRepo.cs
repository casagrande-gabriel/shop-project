using Entities;
using Repositories.Interfaces;

namespace Repositories.Arrays;

public class ArrayCarrierRepo : ICarrierRepo
{
    private Carrier[] _carriers = new Carrier[10];

    int nextIndex = 0;

    private void ArrayResize()
    {
        Carrier[] temp = _carriers;
        _carriers = new Carrier[_carriers.Length * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _carriers[i] = temp[i];
        }
    }

    public void Add(Carrier carrier)
    {
        ArgumentNullException.ThrowIfNull(carrier);

        _carriers[nextIndex] = carrier;

        nextIndex++;
        if (nextIndex >= _carriers.Length)
        {
            ArrayResize();
        }
    }

    public IEnumerable<Carrier> GetAll()
    {
        if (nextIndex == 0) return [];

        Carrier[] result = new Carrier[nextIndex];
        for (int i = 0; i < nextIndex; i++)
        {
            result[i] = _carriers[i];
        }

        return result;
    }

    public Carrier? GetById(int id)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_carriers[i].Id == id) return _carriers[i];
        }

        return null;
    }

    public IEnumerable<Carrier> GetByName(string name)
    {
        if (nextIndex == 0) return [];

        int i;
        int counter = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_carriers[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) counter++;
        }

        Carrier[] carriers = new Carrier[counter];

        int j = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_carriers[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                carriers[j++] = _carriers[i];
            }
        }

        return counter > 0 ? carriers : [];
    }

    public void Remove(Carrier carrier)
    {
        int index = -1;
        int i;

        for (i = 0; i < nextIndex; i++)
        {
            if (_carriers[i] == carrier)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Transportadora não encontrada.");
            return;
        }

        for (; i < nextIndex - 1; i++)
        {
            _carriers[i] = _carriers[i + 1];
        }

        _carriers[nextIndex - 1] = null!;

        nextIndex--;
    }
}
