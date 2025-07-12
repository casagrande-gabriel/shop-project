using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IUserRepo : IRepository<User>
{
    bool Add(Client client, string userName, string password, Role role);
    bool Remove(string userName);
    User? GetUserByUserName(string userName);
    void UpdatePassword(User user, string password);
    bool UserExists(string username);
}
