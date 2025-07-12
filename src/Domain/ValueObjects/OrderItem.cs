using Domain.Entities;

namespace Domain.ValueObjects;

public class OrderItem
{
    public uint Quantity { get; set; }
    public Product Product { get; set; }
    public double TotalPrice { get; set; }

    public OrderItem(Product product, uint quantity)
    {
        Product = product;
        Quantity = quantity;
        TotalPrice = product.Price * Quantity;
    }
}
