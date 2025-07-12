namespace Repositories.Interfaces.Base;

public interface IRepository<T>
{
    void Add(T item);
    void Remove(T item);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}
