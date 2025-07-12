using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class OrderService (
    IOrderRepo _orderRepo,
    IPersistenceService _persistenceService
    ) : IOrderService
{
    public List<Order> GetAllOrders() => [.. _orderRepo.GetAll()];
    public List<Order> GetAllClientOrders(int clientId) => [.. _orderRepo.GetAll().Where(x => x.Client.Id == clientId)];

    public Order? GetOrderById(int id) => _orderRepo.GetById(id);

    public void AddOrder(Order order)
    {
        _orderRepo.Add(order);
        Persist();
    }

    public void RemoveOrder(Order order)
    {
        _orderRepo.Remove(order);
        Persist();
    }

    public void RemoveAllOrders(int clientId)
    {
        var orders = _orderRepo.GetAll()!.Where(o => o.Client.Id == clientId).ToList();
        foreach (var order in orders) _orderRepo.Remove(order);
        Persist();
    }

    public void AddItemToOrder(Order order, Product product, uint quantity)
    {
        if (order is null) throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        if (product is null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");

        order.AddItem(product, quantity);
        Persist();
    }

    public bool RemoveItemFromOrder(Order order, Product product, uint quantity)
    {
        if (order is null) throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        if (product is null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");

        if (order.RemoveItem(product, quantity))
        {
            Persist();
            return true;
        }

        return false;
    }

    public void Persist()
    {
        _persistenceService.Save("pedidos", _orderRepo.GetAll());
    }
}
