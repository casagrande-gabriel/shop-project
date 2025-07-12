using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListUserRepo : BaseListRepository<User>, IUserRepo
{
    public bool Add(Client client, string userName, string password, Role role)
    {
        if (UserExists(userName)) return false;

        _values.Add(new User(client, userName, password, role));

        return true;
    }

    public User? GetUserByUserName(string userName)
    {
        return _values.FirstOrDefault(x => x.UserName.Equals(userName));
    }

    public bool Remove(string userName)
    {
        var user = GetUserByUserName(userName);

        if (user is null) return false;

        return _values.Remove(user);
    }

    public void UpdatePassword(User user, string password)
    {
        var foundUser = GetUserByUserName(user.UserName);

        if (foundUser is null) return;

        foundUser.Password = password;
    }

    public bool UserExists(string username)
    {
        return GetUserByUserName(username) is not null;
    }
}
