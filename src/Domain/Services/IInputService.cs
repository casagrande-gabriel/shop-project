using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public interface IInputService
{
    Address ReadAddress(Address? address = null);
    string ReadString(string query = ": ", string? originalValue = null);
    T? Choose<T>(IEnumerable<T> options) where T : BaseEntity;
}