using Domain.Entities;
using Domain.Enums;

namespace Domain.Services;

public interface IUserService
{
    bool DeleteUser(string userName);
    User? GetUserByUserName(string userName);
    bool Register(Client client, string userName, string password, Role role = Role.User);
    void UpdatePassword(User user, string password);
    bool UserExists(string username);
    void Persist();
}