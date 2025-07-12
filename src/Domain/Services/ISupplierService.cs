using Domain.Entities;

namespace Domain.Services;

public interface ISupplierService
{
    void AddSupplier(Supplier supplier);
    void DeleteSupplier(Supplier supplier);
    List<Supplier> GetAllSuppliers();
    Supplier? GetSupplierById(int supplierId);
    List<Supplier> GetSuppliersByName(string name);
    void ValidateSupplier(Supplier supplier);
    void Persist();
}