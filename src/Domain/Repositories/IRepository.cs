namespace Domain.Repositories;

public interface IRepository<T>
{
    void Add(T item);
    bool Remove(T item);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}
