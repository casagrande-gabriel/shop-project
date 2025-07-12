using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class BaseListRepository<T> : IRepository<T>
    where T : BaseEntity
{
    protected List<T> _values = [];

    public void Add(T item)
    {
        _values.Add(item);
    }

    public IEnumerable<T> GetAll()
    {
        return _values;
    }

    public T? GetById(int id)
    {
        return _values.FirstOrDefault(x => x.Id == id);
    }

    public bool Remove(T item)
    {
        return _values.Remove(item);
    }
}
