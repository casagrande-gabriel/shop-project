using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepository;
    private readonly IPersistenceService _persitenceService;
    public UserService(
        IClientService clientService,
        IPersistenceService persistenceService,
        IUserRepo userRepository)
    {
        _userRepository = userRepository;
        _persitenceService = persistenceService;

        var users = _userRepository.GetAll();

        if (!users.Any(u => u.Role == Role.Admin))
        {
            // Se não houver admin, cria um padrão
            _userRepository.Add(new User(null, "admin", "admin123", Role.Admin));
            Persist();
        }
    }

    public void UpdatePassword(User user, string password)
    {
        _userRepository.UpdatePassword(user, password);
        Persist();

    }

    public bool Register(Client client, string userName, string password, Role role = Role.User)
    {
        if (_userRepository.Add(client, userName, password, role))
        {
            Persist();
            return true;
        }

        return false;
    }

    public bool DeleteUser(string userName)
    {
        if (_userRepository.Remove(userName))
        {
            Persist();
            return true;
        }

        return false;
    }

    public User? GetUserByUserName(string userName)
    {
        return _userRepository.GetUserByUserName(userName);
    }

    public bool UserExists(string username)
    {
        return _userRepository.UserExists(username);
    }

    public void Persist()
    {
        _persitenceService.Save("usuarios", _userRepository.GetAll());
    }
}
