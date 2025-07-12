using Entities;
using Repositories.Interfaces;

namespace Repositories.Arrays;

public class ArrayOrderRepo : IOrderRepo
{
    private Order[] _orders = new Order[10];

    int nextIndex = 0;

    private void ArrayResize()
    {
        Order[] temp = _orders;
        _orders = new Order[_orders.Length * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _orders[i] = temp[i];
        }
    }

    public void Add(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orders[nextIndex] = order;

        nextIndex++;
        if (nextIndex >= _orders.Length)
        {
            ArrayResize();
        }
    }

    public IEnumerable<Order> GetAll()
    {
        if (nextIndex == 0) return [];

        Order[] result = new Order[nextIndex];
        for (int i = 0; i < nextIndex; i++)
        {
            result[i] = _orders[i];
        }

        return result;
    }

    public Order? GetById(int id)
    {
        if (nextIndex == 0) return null;

        for (int i = 0; i < nextIndex; i++)
        {
            if (_orders[i].Id == id) return _orders[i];
        }

        return null;
    }

    public void Remove(Order order)
    {
        int index = -1;
        int i;

        for (i = 0; i < nextIndex; i++)
        {
            if (_orders[i] == order)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("Pedido não encontrado.");
            return;
        }

        for (; i < nextIndex - 1; i++)
        {
            _orders[i] = _orders[i + 1];
        }

        _orders[nextIndex - 1] = null!;

        nextIndex--;
    }
}
