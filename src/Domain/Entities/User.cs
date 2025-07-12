using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public Client? Client { get; set; }

    public User(Client? client, string userName, string password, Role role = Role.User) : base()
    {
        Client = client;
        UserName = userName;
        Password = password;
        Role = role;
    }

    [JsonConstructor]
    public User(int id, Client? client, string userName, string password, Role role = Role.User) : base(id)
    {
        Id = id;
        Client = client;
        UserName = userName;
        Password = password;
        Role = role;
    }
}
