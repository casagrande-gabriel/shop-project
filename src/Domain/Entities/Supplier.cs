using System.Text.Json.Serialization;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }

    public Supplier(string name, string description, string phoneNumber, string email, Address address) : base()
    {
        Name = name;
        Description = description;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    [JsonConstructor]
    public Supplier(int id, string name, string description, string phoneNumber, string email, Address address) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | Phone: {PhoneNumber}\nEndereço: {Address}";
    }
}
