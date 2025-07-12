using Domain.Entities;

namespace Domain.Services;

public interface IOrderService
{
    void AddItemToOrder(Order order, Product product, uint quantity);
    void AddOrder(Order order);
    List<Order> GetAllOrders();
    List<Order> GetAllClientOrders(int clientId);
    Order? GetOrderById(int id);
    void RemoveAllOrders(int clientId);
    bool RemoveItemFromOrder(Order order, Product product, uint quantity);
    void RemoveOrder(Order order);
    void Persist();
}