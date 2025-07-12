using Models.Enums;

namespace Models.Controllers;

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