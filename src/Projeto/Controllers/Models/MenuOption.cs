using Domain.Enums;

namespace Projeto.Controllers.Models;

public sealed record MenuOption(
    string Label,
    Action Action,
    Role? Role = null,
    RoleAccessMode Mode = RoleAccessMode.AtLeast)
{
    public override string ToString()
    {
        return Label;
    }
}