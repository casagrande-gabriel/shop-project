using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;

namespace Application.Services;

public class AuthService(IUserService _userService) : IAuthService
{
    public User? LoggedUser { get; private set; }

    public Client? Client { get => LoggedUser?.Client; }

    public bool DoesUserHaveAccess(Role? requiredRole, RoleAccessMode mode = RoleAccessMode.AtLeast)
    {
        if (!IsLogged) return false;

        return mode switch
        {
            RoleAccessMode.AtLeast => LoggedUser!.Role >= requiredRole,
            RoleAccessMode.Exactly => LoggedUser!.Role == requiredRole,
            _ => false
        };
    }

    public bool Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

        var user = _userService.GetUserByUserName(username);

        if (user == null || password != user.Password) return false;

        LoggedUser = user;
        return true;
    }

    public bool ChangePassword(User user, string oldPassword, string newPassword)
    {
        if (oldPassword != user.Password) throw new ArgumentException("Senha antiga incorreta.");

        _userService.UpdatePassword(user, newPassword);
        return true;
    }

    public void LogOut()
    {
        LoggedUser = null;
    }

    public bool IsAdmin => IsLogged && LoggedUser.Role == Role.Admin;

    [MemberNotNullWhen(true, nameof(LoggedUser))]
    public bool IsLogged => LoggedUser is not null;

    [MemberNotNullWhen(true, nameof(Client))]
    public bool IsClient => IsLogged && LoggedUser.Role == Role.User && Client is not null;
}
