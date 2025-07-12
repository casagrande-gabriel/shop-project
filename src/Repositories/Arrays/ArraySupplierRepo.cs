using Entities;
using Repositories.Interfaces;

namespace Repositories.Arrays;

public class ArraySupplierRepo : ISupplierRepo
{
    private Supplier[] _suppliers = new Supplier[10];

    int nextIndex = 0;

    private void ArrayResize()
    {
        Supplier[] temp = _suppliers;
        _suppliers = new Supplier[_suppliers.Length * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _suppliers[i] = temp[i];
        }
    }

    public void Add(Supplier supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);

        _suppliers[nextIndex] = supplier;

        nextIndex++;
        if (nextIndex >= _suppliers.Length)
        {
            ArrayResize();
        }
    }

    public IEnumerable<Supplier> GetAll()
    {
        if (nextIndex == 0) return [];

        Supplier[] result = new Supplier[nextIndex];
        for (int i = 0; i < nextIndex; i++)
        {
            result[i] = _suppliers[i];
        }

        return result;
    }

    public Supplier? GetById(int id)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_suppliers[i].Id == id) return _suppliers[i];
        }

        return null;
    }

    public IEnumerable<Supplier> GetByName(string name)
    {
        if (nextIndex == 0) return [];

        int i;
        int counter = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_suppliers[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) counter++;
        }

        Supplier[] suppliers = new Supplier[counter];

        int j = 0;

        for (i = 0; i < nextIndex; i++)
        {
            if (_suppliers[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                suppliers[j++] = _suppliers[i];
            }
        }

        return counter > 0 ? suppliers : [];
    }

    public void Remove(Supplier supplier)
    {
        int index = -1;
        int i;

        for (i = 0; i < nextIndex; i++)
        {
            if (_suppliers[i] == supplier)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Fornecedor não encontrado.");
            return;
        }

        for (; i < nextIndex - 1; i++)
        {
            _suppliers[i] = _suppliers[i + 1];
        }

        _suppliers[nextIndex - 1] = null!;

        nextIndex--;
    }
}
