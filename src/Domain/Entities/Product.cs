using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public uint Stock { get; set; }
    public Supplier Supplier { get; set; }

    public Product(string name, double price, uint stock, Supplier supplier) : base()
    {
        Name = name;
        Price = price;
        Stock = stock;
        Supplier = supplier;
    }

    [JsonConstructor]
    public Product(int id, string name, double price, uint stock, Supplier supplier) : base(id)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        Supplier = supplier;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | Preço: {Price:C2} | Estoque: {Stock} | Fornecedor: {Supplier.Name}.";
    }
}
