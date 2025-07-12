using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public Client Client { get; set; }
    public DateTime OrderDateTime { get; set; }
    public DateTime DeliveryDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Carrier Carrier { get; set; }
    public double DeliveryFee { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public double TotalPrice { get; set; }

    public Order(Client client, Carrier carrier, double deliveryFee) : base()
    {
        OrderDateTime = DateTime.Now;
        Status = OrderStatus.Processando;
        Client = client;
        Carrier = carrier;
        DeliveryFee = deliveryFee;
        
        UpdatePrice();
    }

    [JsonConstructor]
    public Order(int id, Client client, Carrier carrier, double deliveryFee, double totalPrice) : base(id)
    {
        Id = id;
        OrderDateTime = DateTime.Now;
        Status = OrderStatus.Processando;
        Client = client;
        Carrier = carrier;
        DeliveryFee = deliveryFee;
        TotalPrice = totalPrice;
    }

    public void AddItem(Product product, uint quantity)
    {
        OrderItem? item = Items.FirstOrDefault(i => i.Product == product);

        if (item is not null)
        {
            item.Quantity += quantity;
            return;
        }

        item = new(product, quantity);
        Items.Add(item);

        UpdatePrice();
    }

    public bool RemoveItem(Product product, uint quantity)
    {
        OrderItem? item = Items.FirstOrDefault(i => i.Product == product);

        if (item is not null)
        {
            if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
                return true;
            }

            Items.Remove(item);
            
            UpdatePrice();
            return true;
        }

        return false;
    }

    public void UpdatePrice()
    {
        TotalPrice = 0;

        foreach (var item in Items)
        {
            TotalPrice += item.TotalPrice;
        }
    }

    public override string? ToString()
    {
        return $"Pedido {Id} | Data de entraga {DeliveryDateTime:dd/MM} | Total: {TotalPrice:C2}";
    }
}
