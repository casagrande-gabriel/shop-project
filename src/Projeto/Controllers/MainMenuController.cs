using Domain.Enums;
using Domain.Services;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

public class MainMenuController(
    INavigationService NavigationService,
    IAuthService _authService,
    IController _loginController,
    IController _registerController,
    IController _shoppingCartController,
    IController _orderController,
    IController _clientController,
    IController _supplierController,
    IController _productController,
    IController _carrierController,
    IShoppingCartService _shoppingCartService) 
    : BaseMenuController(NavigationService)
{
    public override string Title => _authService.IsLogged
        ? $"Seja bem-vindo, {(_authService.IsClient ? _authService.Client.FirstName : _authService.LoggedUser?.UserName)}"
        : "Seja bem-vindo. Entre ou se registre.";

    private MenuOption[] Options => [
        new ("Login", Login),
        new ("Registrar", Register),
        new ("Carrinho de compras", ShoppingCart, Role.User, RoleAccessMode.Exactly),
        new ("Pedidos", Orders, Role.User),
        new ("Clientes", Clients, Role.Employee),
        new ("Fornecedores", Suppliers, Role.Admin),
        new ("Produtos", Products, Role.Employee),
        new ("Transportadoras", Carriers, Role.Admin),
        new ("Sair", LogOut, Role.User)
    ];

    private void Login() => GoTo(_loginController);
    private void Register() => GoTo(_registerController);
    private void ShoppingCart() => GoTo(_shoppingCartController);
    private void Clients() => GoTo(_clientController);
    private void Orders() => GoTo(_orderController);
    private void Suppliers() => GoTo(_supplierController);
    private void Products() => GoTo(_productController);
    private void Carriers() => GoTo(_carrierController);

    private void LogOut()
    {
        _shoppingCartService.Clear();

        _authService.LogOut();
       
        ShowText("Você foi desconectado.");
    }

    public override IList<MenuOption> GetOptions()
    {
        if (!_authService.IsLogged) return [.. Options.Where(x => x.Role is null)];

        return [.. Options.Where(x => _authService.DoesUserHaveAccess(x.Role, x.Mode))];
    }
}