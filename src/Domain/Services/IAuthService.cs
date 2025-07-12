using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Services;

public interface IAuthService
{
    User? LoggedUser { get; }
    Client? Client { get; }
    bool IsAdmin { get; }

    [MemberNotNullWhen(true, nameof(Client))]
    bool IsClient { get; }

    [MemberNotNullWhen(true, nameof(LoggedUser))]
    bool IsLogged { get; }
    bool ChangePassword(User user, string oldPassword, string newPassword);
    bool DoesUserHaveAccess(Role? requiredRole, RoleAccessMode mode = RoleAccessMode.AtLeast);
    bool Login(string username, string password);
    void LogOut();
}