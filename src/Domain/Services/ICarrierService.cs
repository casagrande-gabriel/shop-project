using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services;

public interface ICarrierService
{
    void AddCarrier(Carrier carrier);
    List<Carrier> GetAllCarriers();
    Carrier? GetCarrierById(int id);
    List<Carrier> GetCarriersByName(string name);
    void RemoveCarrier(Carrier carrier);
    void Persist();
}