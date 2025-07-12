using System.Text.Json.Serialization;
using Models.Enums;

namespace Entities;

public class User
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public Client Client { get; set; }

    public User(Client client, string userName, string password)
    {
        Client = client;
        UserName = userName;
        Password = password;
        Role = Role.User;
    }

    [JsonConstructor]
    public User(Client client, string userName, string password, Role role)
    {
        Client = client;
        UserName = userName;
        Password = password;
        Role = role;
    }
}
