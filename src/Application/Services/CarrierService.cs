using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class CarrierService(ICarrierRepo _carrierRepo, IPersistenceService _persistenceService) : ICarrierService
{
    public void AddCarrier(Carrier carrier)
    {
        _carrierRepo.Add(carrier);
        Persist();
    }

    public void RemoveCarrier(Carrier carrier)
    {
        _carrierRepo.Remove(carrier);
        Persist();
    }

    public List<Carrier> GetAllCarriers() => [.. _carrierRepo.GetAll()];
    public List<Carrier> GetCarriersByName(string name) => [.. _carrierRepo.GetByName(name)];
    public Carrier? GetCarrierById(int id) => _carrierRepo.GetById(id);
    public void Persist() => _persistenceService.Save("transportadoras", _carrierRepo.GetAll());
}
