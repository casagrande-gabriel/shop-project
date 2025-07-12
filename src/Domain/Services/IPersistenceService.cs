namespace Domain.Services;

public interface IPersistenceService
{
    bool Save(string key, object data);
    T? Load<T>(string key);
}
