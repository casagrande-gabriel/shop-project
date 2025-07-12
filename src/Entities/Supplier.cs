using Models;

namespace Entities;

public class Supplier
{
    private static int _lastId = 1;
    public int Id { get; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public List<Product> Products { get; set; } = [];

    public Supplier(string name, string description, string phoneNumber, string email, Address address)
    {
        Id = _lastId++;
        Name = name;
        Description = description;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    public void AddProduct(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product), "O produto não pode ser nulo.");
        if (Products.Any(p => p.Id == product.Id))
            throw new InvalidOperationException("Produto já está associado a este fornecedor.");
        Products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        if (product is null) throw new ArgumentNullException(nameof(product), "O produto não pode ser nulo.");
        if (!Products.Remove(product))
            throw new InvalidOperationException("Produto não encontrado entre os produtos deste fornecedor.");
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name}\n Endereço: {Address}";
    }
}
