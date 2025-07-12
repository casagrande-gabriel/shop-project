using Entities;
using Repositories.Interfaces;

namespace Repositories.Arrays;

public class ArrayProductRepo : IProductRepo
{
    private Product[] _products = new Product[10];

    int nextIndex = 0;

    private void ArrayResize()
    {
        Product[] temp = _products;
        _products = new Product[_products.Length * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _products[i] = temp[i];
        }
    }

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products[nextIndex] = product;

        nextIndex++;
        if (nextIndex >= _products.Length)
        {
            ArrayResize();
        }
    }

    public IEnumerable<Product> GetAll()
    {
        if (nextIndex == 0) return [];

        Product[] result = new Product[nextIndex];
        for (int i = 0; i < nextIndex; i++)
        {
            result[i] = _products[i];
        }

        return result;
    }

    public Product? GetById(int id)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_products[i].Id == id) return _products[i];
        }

        return null;
    }

    public Product? GetByName(string name)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_products[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) return _products[i];
        }

        return null;
    }

    public void Remove(Product product)
    {
        int index = -1;
        int i;

        for (i = 0; i < nextIndex; i++)
        {
            if (_products[i] == product)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        for (; i < nextIndex - 1; i++)
        {
            _products[i] = _products[i + 1];
        }

        _products[nextIndex - 1] = null!;

        nextIndex--;
    }
}
