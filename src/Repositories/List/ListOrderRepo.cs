using Entities;
using Repositories.Interfaces;

namespace Repositories.List;

public class ListOrderRepo : IOrderRepo
{
    private readonly List<Order> _orders = [];

    public IEnumerable<Order> GetAll() => _orders;

    public Order? GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

    public void Add(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _orders.Add(order);
    }

    public void Remove(Order item)
    {
        throw new NotImplementedException();
    }
}
