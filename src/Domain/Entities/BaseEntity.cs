namespace Domain.Entities;

public class BaseEntity
{
    private static int _lastId = 0;
    public int Id { get; set; }

    public BaseEntity()
    {
        Id = ++_lastId;
    }

    public BaseEntity(int id)
    {
        _lastId = Math.Max(_lastId, id);
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity otherEntity)
        {
            return false;
        }

        return Id == otherEntity.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}
