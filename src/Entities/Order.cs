using Models.Enums;

namespace Entities;

public class Order
{
    private static int _lastId = 1;
    public int Id { get; init; }
    public Client Client { get; init; }
    public DateTime OrderDateTime { get; set; }
    public DateTime DeliveryDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Carrier Carrier { get; set; }
    public double DeliveryFee { get; set; }
    public readonly List<OrderItem> Items = [];

    public Order(Client client, Carrier carrier, double deliveryFee)
    {
        Id = _lastId++;
        OrderDateTime = DateTime.Now;
        Status = OrderStatus.Processando;
        Client = client;
        Carrier = carrier;
        DeliveryFee = deliveryFee;
    }

    public void AddItem(Product product, uint quantity)
    {
        OrderItem? item = Items.FirstOrDefault(i => i.Product == product);

        if (item is not null)
        {
            item.Quantity += quantity;
            item.TotalPrice = product.Price * item.Quantity;
            return;
        }

        item = new(product, quantity);
        Items.Add(item);
    }

    public void RemoveItem(Product product, uint quantity)
    {
        OrderItem? item = Items.FirstOrDefault(i => i.Product == product);

        if (item is not null)
        {
            if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
                item.TotalPrice = item.Product.Price * item.Quantity;
                return;
            }

            Items.Remove(item);
            return;
        }

        Console.Clear();
        Console.WriteLine("Item não encontrado no pedido.");
        Thread.Sleep(1500);
        Console.Clear();
    }
}
