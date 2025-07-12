using Entities;
using Repositories.Interfaces;

namespace Repositories.List;

public class ListSupplierRepo : ISupplierRepo
{
    private readonly List<Supplier> _supplierList = [];

    public IEnumerable<Supplier> GetAll() => _supplierList;

    public Supplier? GetById(int id) => _supplierList.FirstOrDefault(s => s.Id == id);

    public IEnumerable<Supplier> GetByName(string name) =>
        [.. _supplierList.Where(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))];

    public void Add(Supplier supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        _supplierList.Add(supplier);
    }

    public void Remove(Supplier supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        _supplierList.Remove(supplier);
    }
}