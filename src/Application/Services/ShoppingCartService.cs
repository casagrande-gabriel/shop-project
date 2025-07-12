using Domain.Entities;
using Domain.Services;

namespace Application.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly Dictionary<Product, uint> _cart = [];

    public void AddQuantity(Product product, uint quantity)
    {
        if (quantity > product.Stock) quantity = product.Stock;

        if (_cart.TryGetValue(product, out _))
        {
            _cart[product] += quantity;
        }
        else _cart[product] = quantity;

        product.Stock -= quantity;
    }

    public void Clear()
    {
        foreach (var (product, quantity) in _cart)
        {
            product.Stock += quantity;
        }

        _cart.Clear();
    }

    public uint GetQuantity(Product product)
    {
        return _cart.GetValueOrDefault(product, 0u);
    }

    public double GetTotalPrice()
    {
        return _cart.Sum(value => value.Key.Price * value.Value);
    }

    public bool IsEmpty()
    {
        return _cart.Count == 0;
    }

    public void Print()
    {
        foreach (var item in _cart)
        {
            Console.WriteLine($"Produto {item.Key.Id}: {item.Key.Name} | Quantidade: {item.Value} | Preço unitário: {item.Key.Price:C}");
        }
    }

    public void Remove(Product product)
    {
        if (_cart.TryGetValue(product, out var quantity))
        {
            product.Stock += quantity;
            _cart.Remove(product);
        }
    }

    public void RemoveQuantity(Product product, uint quantity)
    {
        if (_cart.TryGetValue(product, out var currentQuantity))
        {
            if (quantity >= currentQuantity)
            {
                product.Stock += currentQuantity;
                _cart.Remove(product);
            }
            else
            {
                product.Stock += quantity;
                _cart[product] = currentQuantity - quantity;
            }
        }
    }

    public void ConcludeSale()
    {
        _cart.Clear();
    }

    public IEnumerable<(Product, uint)> GetCart()
    {
        return _cart.Select(x => (x.Key, x.Value));
    }
}
