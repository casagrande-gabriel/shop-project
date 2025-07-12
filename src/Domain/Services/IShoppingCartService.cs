using Domain.Entities;

namespace Domain.Services;

public interface IShoppingCartService
{
    bool IsEmpty();
    void AddQuantity(Product product, uint quantity);
    void RemoveQuantity(Product product, uint quantity);
    void Remove(Product product);
    uint GetQuantity(Product product);
    IEnumerable<(Product, uint)> GetCart();
    void ConcludeSale();
    void Clear();
    void Print();
    double GetTotalPrice();
}
