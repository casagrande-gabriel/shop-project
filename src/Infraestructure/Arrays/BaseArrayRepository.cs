using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class BaseArrayRepository<T> : IRepository<T>
    where T : BaseEntity
{
    protected int _index = 0;
    protected T[] _values = [];

    private void Resize()
    {
        T[] temp = _values;
        _values = new T[Math.Max(_values.Length, 1) * 2];

        for (int i = 0; i < temp.Length; i++)
        {
            _values[i] = temp[i];
        }
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_index >= _values.Length)
        {
            Resize();
        }

        _values[_index++] = item;
    }

    public IEnumerable<T> GetBy(Predicate<T> predicate)
    {
        int count = 0;

        for (int i = 0; i < _index; i++)
        {
            if (predicate(_values[i])) count++;
        }

        T[] values = new T[count];

        for (int i = 0; i < _index; i++)
        {
            if (predicate(_values[i])) values[--count] = _values[i];
        }

        return values;
    }

    public T? GetById(int id)
    {
        return GetBy(x => x.Id == id).FirstOrDefault();
    }

    public bool Remove(T item)
    {
        int i;

        for (i = 0; i < _index; i++)
        {
            if (_values[i]?.Equals(item) is true)
            {
                break;
            }
        }

        if (i == -1)
        {
            return false;
        }

        for (; i < _index - 1; i++)
        {
            _values[i] = _values[i + 1];
        }

        _index--;

        return true;
    }

    public IEnumerable<T> GetAll()
    {
        return GetBy(_ => true);
    }
}
