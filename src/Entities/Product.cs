namespace Entities;

public class Product
{
    private static int _lastId = 1;
    public int Id { get; init; }
    public string Name { get; set; }
    public double Price { get; set; }
    public uint Stock { get; set; }
    public Supplier Supplier { get; set; }

    public Product(string name, double price, uint stock, Supplier supplier)
    {
        Id = _lastId++;
        Name = name;
        Price = price;
        Stock = stock;
        Supplier = supplier;
        supplier.AddProduct(this);
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | Preço: {Price:C2} | Estoque: {Stock} | Fornecedor: {Supplier.Name}.";
    }
}
