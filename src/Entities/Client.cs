using System.Text.Json.Serialization;
using Models;

namespace Entities;

public class Client
{
    private static int _lastId = 1;
    public int Id { get; init; }
    public string Name { get; set; }
    public string CPF { get; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public double Distance { get; set; }

    public Client(string name, string cpf, string phone, string email, Address address, double distance )
    {
        Id = _lastId++;
        Name = name;
        CPF = cpf;
        Phone = phone;
        Email = email;
        Address = address;
        Distance = distance;
    }

    [JsonConstructor]
    private Client(int id, string name, string cpf, string phone, string email, Address address, double distance)
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
}
