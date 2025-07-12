using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Projeto.Controllers.Base;
using Projeto.Controllers.Models;

namespace Projeto.Controllers;

internal class OrderController(
    INavigationService _navigationService,
    IAuthService _authService,
    IOrderService _orderService)
    : BaseMenuController(_navigationService)
{
    public override string Title => "Cadastro de pedidos";

    public MenuOption[] Options => [
        new ("Listar todos os pedidos", WriteOrders, Role.Employee),
        new ("Listar todos os pedidos", ClientSideWriteOrders, Role.User, RoleAccessMode.Exactly),
        new ("Listar pedidos por data", WriteOrdersByDate, Role.User),
        new ("Listar pedido por ID", WriteOrdersById, Role.User),
        new ("Modificar status do pedido", ModifyOrderStatus, Role.User)
    ];

    public override IList<MenuOption> GetOptions()
    {
        if (!_authService.IsLogged) return [.. Options.Where(x => x.Role is null)];

        return [.. Options.Where(x => _authService.DoesUserHaveAccess(x.Role, x.Mode))];
    }

    public void WriteOrders()
    {
        List<Order> orders = _orderService.GetAllOrders();

        if (orders.Count == 0)
        {
            ShowText("Nenhum pedido cadastrado.");
            return;
        }

        do
        {
            Console.Clear();
            Console.WriteLine("Pedidos:");
            Console.WriteLine("\n-----------------------------------------------------------------------\n");

            PrintOrders(orders);

            Console.Write("Pressione Enter para continuar: ");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
    }

    public void ModifyOrderStatus()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Digite o ID do pedido que deseja modificar o status ou pressione Enter para voltar: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Clear();
                return;
            }

            if (!int.TryParse(input, out int orderId))
            {
                ShowText("ID inv�lido.");
                continue;
            }

            bool isClient = _authService.IsClient;

            Order? order = _orderService.GetOrderById(orderId);

            if (isClient && order?.Client != _authService.Client)
            {
                ShowText("Voc� n�o tem permiss�o para modificar este pedido.");
                continue;
            }

            if (order is null)
            {
                ShowText("Pedido n�o encontrado.");
                continue;
            }

            if (order.Status == OrderStatus.Cancelado)
            {
                ShowText("Esse pedido foi cancelado.");
                return;
            }

            if (order.Status == OrderStatus.Entregue)
            {
                ShowText("Esse pedido j� foi entregue.");
                return;
            }

            if (isClient)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"ID: {order.Id}\n" +
                    $"Data do pedido: {order.OrderDateTime}\n" +
                    $"Status: {order.Status}\n");

                    Console.WriteLine("1 - Confirmar entrega");
                    Console.WriteLine("2 - Cancelar pedido");
                    Console.WriteLine("0 - Voltar");
                    Console.Write("\nEscolha uma op��o: ");

                    input = Console.ReadLine()!;

                    switch (input)
                    {
                        case "0":
                            break;
                        case "1":
                            order.Status = OrderStatus.Entregue;
                            order.DeliveryDateTime = DateTime.Now;
                            ShowText("Pedido atualizado para entregue.");
                            return;
                        case "2":
                            order.Status = OrderStatus.Cancelado;
                            foreach (var item in order.Items)
                            {
                                item.Product.Stock += item.Quantity; // Adiciona a quantidade de volta ao estoque
                                item.Quantity = 0; // Zera a quantidade dos itens
                            }
                            ShowText("Pedido cancelado.");
                            return;
                        default:
                            ShowText("Op��o inv�lida. Tente novamente.");
                            continue;
                    }
                    return;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"ID: {order.Id}\n" +
                    $"Data do pedido: {order.OrderDateTime}\n" +
                    $"Status: {order.Status}\n");

                Console.WriteLine("1 - Enviar pedido para entrega");
                Console.WriteLine("2 - Confirmar entrega");
                Console.WriteLine("3 - Cancelar pedido");
                Console.WriteLine("0 - Voltar");
                Console.Write("\nEscolha uma op��o: ");

                input = Console.ReadLine()!;

                switch (input)
                {
                    case "0":
                        break;
                    case "1":
                        order.Status = OrderStatus.Enviado;
                        ShowText("Pedido enviado para entrega.");
                        return;
                    case "2":
                        order.Status = OrderStatus.Entregue;
                        order.DeliveryDateTime = DateTime.Now;
                        ShowText("Pedido atualizado para entregue.");
                        return;
                    case "3":
                        order.Status = OrderStatus.Cancelado;
                        foreach (var item in order.Items)
                        {
                            item.Product.Stock += item.Quantity; // Adiciona a quantidade de volta ao estoque
                            item.Quantity = 0; // Zera a quantidade dos itens
                        }
                        ShowText("Pedido cancelado.");
                        return;
                    default:
                        ShowText("Op��o inv�lida. Tente novamente.");
                        continue;
                }
                return;
            }
        }
    }

    public void WriteOrdersById()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Digite o ID do pedido ou pressione Enter para voltar: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Clear();
                return;
            }

            if (!int.TryParse(input, out int orderId))
            {
                ShowText("ID inv�lido.");
                continue;
            }

            Order? order = _orderService.GetOrderById(orderId);

            if (order is null)
            {
                ShowText("Pedido n�o encontrado.");
                continue;
            }

            if (_authService.IsClient && order.Client != _authService.Client)
            {
                ShowText("Voc� n�o tem permiss�o para visualizar este pedido.");
                continue;
            }

            do
            {
                Console.Clear();
                if (!_authService.IsClient) Console.WriteLine($"Client: {order.Client!.Name}");
                Console.WriteLine($"ID: {order.Id}\n" +
                                $"Data do pedido: {order.OrderDateTime}");
                if (order.Status is OrderStatus.Entregue) Console.WriteLine($"Data de entrega: {order.DeliveryDateTime}");
                else Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Transportadora: {order.Carrier.Name}\n");
                Console.WriteLine("Produtos:\n");
                double sum = 0;
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"{item.Product.Name} | " +
                        $"Pre�o un.: {item.Product.Price:C} | " +
                        $"Quantidade: {item.Quantity} | " +
                        $"Pre�o total: {item.TotalPrice:C}");
                    sum += item.TotalPrice;
                }
                Console.WriteLine($"Total: {sum:C}");
                Console.WriteLine($"\nTaxa de entrega: {order.DeliveryFee:C}");
                sum += order.DeliveryFee;
                Console.WriteLine($"Subtotal: {sum:C}");
                Console.WriteLine("\n-----------------------------------------------------------------------\n");
                Console.Write("Pressione Enter para continuar: ");
            } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
            break;
        }
    }

    public void WriteOrdersByDate()
    {
        Console.Clear();
        Console.Write("Digite a data inicial (dd/mm/aaaa) ou pressione Enter para voltar: ");
        string? startDateInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(startDateInput))
        {
            Console.Clear();
            return;
        }

        if (!DateTime.TryParse(startDateInput, out DateTime startDate))
        {
            ShowText("Data inv�lida.");
            return;
        }

        Console.Write("Digite a data final (dd/mm/aaaa) ou pressione Enter para voltar: ");
        string? endDateInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(endDateInput))
        {
            Console.Clear();
            return;
        }

        if (!DateTime.TryParse(endDateInput, out DateTime endDate))
        {
            ShowText("Data inválida.");
            return;
        }

        if (endDate < startDate)
        {
            ShowText("A data final não pode ser anterior à data inicial.");
            return;
        }

        List<Order> orders;

        if (_authService.IsClient)
        {
            orders = [.. _orderService.GetAllOrders().
                Where(o => o.Client == _authService.Client &&
                           o.OrderDateTime.Date >= startDate.Date &&
                           o.OrderDateTime.Date <= endDate.Date)];

            if (orders.Count == 0)
            {
                ShowText("Nenhum pedido encontrado nesse per�odo.");
                return;
            }

            do
            {
                Console.Clear();
                Console.WriteLine($"Pedidos do cliente {_authService.Client!.Name} entre {startDate:dd/MM/yyyy} e {endDate:dd/MM/yyyy}:");
                Console.WriteLine("\n-----------------------------------------------------------------------\n");

                PrintOrders(orders, Role.User);
                Console.Write("Pressione Enter para continuar");
            } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
            return;
        }

        do
        {
            orders = [.. _orderService.GetAllOrders().
            Where(o => o.OrderDateTime.Date >= startDate.Date &&
                       o.OrderDateTime.Date <= endDate.Date)];

            if (orders.Count == 0)
            {
                ShowText("Nenhum pedido encontrado nesse período.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Pedidos entre {startDate:dd/MM/yyyy} e {endDate:dd/MM/yyyy}");

            Console.WriteLine("\n-----------------------------------------------------------------------\n");

            PrintOrders(orders);
            Console.Write("Pressione Enter para continuar: ");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
    }

    public void ClientSideWriteOrders()
    {
        List<Order>? orders = [.. _orderService.GetAllOrders().Where(o => o.Client == _authService.Client)];

        if (orders.Count == 0)
        {
            ShowText("Ainda não há pedidos.");
            return;
        }

        do
        {
            Console.Clear();
            Console.WriteLine("Pedidos:");

            Console.WriteLine("\n-----------------------------------------------------------------------\n");

            PrintOrders(orders, Role.User);

            Console.Write("Pressione Enter para continuar: ");
        } while (Console.ReadKey(true).Key != ConsoleKey.Enter);
    }

    private static void PrintOrders(List<Order> orders, Role role = Role.Admin)
    {
        if (Role.User == role)
        {
            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.Id}\n" +
                    $"Data do pedido: {order.OrderDateTime}\n" +
                    $"Status: {order.Status}");

                if (order.Status == OrderStatus.Entregue)
                {
                    Console.WriteLine($"Data de entrega: {order.DeliveryDateTime}");
                }

                Console.WriteLine($"Transportadora: {order.Carrier.Name}\n");

                Console.WriteLine("Produtos:\n");

                double sum = 0;

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"{item.Product.Name} | " +
                        $"Preço un.: {item.Product.Price:C} | " +
                        $"Quantidade: {item.Quantity} | " +
                        $"Preço total: {item.TotalPrice:C}");
                    sum += item.TotalPrice;
                }
                Console.WriteLine($"Total: {sum:C}");

                Console.WriteLine($"\nTaxa de entrega: {sum:C}");

                sum += order.DeliveryFee;
                Console.WriteLine($"Subtotal: {sum:C}");
                Console.WriteLine("\n-----------------------------------------------------------------------\n");
            }
            return;
        }

        foreach (var order in orders)
        {
            Console.WriteLine($"Nome: {order.Client.Name}\n" +
                        $"ID: {order.Id}\n" +
                        $"Data do pedido: {order.OrderDateTime}");
            if (order.Status is OrderStatus.Entregue) Console.WriteLine($"Data de entrega: {order.DeliveryDateTime}");
            else Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Transportadora: {order.Carrier.Name}\n");
            Console.WriteLine("Produtos:\n");
            double sum = 0;
            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Product.Name} | " +
                    $"Pre�o un.: {item.Product.Price:C} | " +
                    $"Quantidade: {item.Quantity} | " +
                    $"Pre�o total: {item.TotalPrice:C}");
                sum += item.TotalPrice;
            }
            Console.WriteLine($"Total: {sum:C}");
            Console.WriteLine($"\nTaxa de entrega: {order.DeliveryFee:C}");
            sum += order.DeliveryFee;
            Console.WriteLine($"Subtotal: {sum:C}");
            Console.WriteLine("\n-----------------------------------------------------------------------\n");
        }
    }
}
