using System.Text.Json.Serialization;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Client : BaseEntity
{
    public string Name { get; set; }
    public string CPF { get; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public double Distance { get; set; }

    public Client(string name, string cpf, string phone, string email, Address address, double distance) : base()
    {
        Name = name;
        CPF = cpf;
        Phone = phone;
        Email = email;
        Address = address;
        Distance = distance;
    }

    [JsonConstructor]
    public Client(int id, string name, string cpf, string phone, string email, Address address, double distance) : base(id)
    {
        Id = id;
        Name = name;
        CPF = cpf;
        Phone = phone;
        Email = email;
        Address = address;
        Distance = distance;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | CPF: {CPF} | Telefone: {Phone} | E-mail: {Email}";
    }

    public string FirstName
    {
        get => Name.Split(" ").First();
    }
}
