using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class SupplierService(
    ISupplierRepo _supplierRepo,
    IProductService _productService,
    IPersistenceService _persistenceService,
    IValidationService _validationService) : ISupplierService
{
    public void ValidateSupplier(Supplier supplier)
    {
        if (string.IsNullOrEmpty(supplier.Name))
            throw new ArgumentException("O nome do fornecedor deve ser informado", nameof(supplier));

        if (string.IsNullOrWhiteSpace(supplier.Description)) supplier.Description = null;

        _validationService.ValidateTelephone(supplier.PhoneNumber);
        _validationService.ValidateEmail(supplier.Email);
        _validationService.ValidateAddress(supplier.Address);
    }

    public void AddSupplier(Supplier supplier)
    {
        ValidateSupplier(supplier);
        _supplierRepo.Add(supplier);
        Persist();
    }

    public Supplier? GetSupplierById(int supplierId) => _supplierRepo.GetById(supplierId);

    public void DeleteSupplier(Supplier supplier)
    {
        var products = _productService.GetAllProductsOfSupplier(supplier);

        foreach (var product in products)
        {
            _productService.RemoveProduct(product);
        }

        _supplierRepo.Remove(supplier);
        Persist();
    }

    public List<Supplier> GetSuppliersByName(string name) => [.. _supplierRepo.GetByName(name)];

    public List<Supplier> GetAllSuppliers() => [.. _supplierRepo.GetAll()];

    public void Persist()
    {
        _persistenceService.Save("fornecedores", _supplierRepo.GetAll());
    }
}
