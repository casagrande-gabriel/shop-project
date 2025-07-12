using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class ClientService (
    IClientRepo _clientRepo,
    IOrderService _orderService,
    IPersistenceService _persistenceService) : IClientService
{
    public void AddClient(Client client)
    {
        _clientRepo.Add(client);
        Persist();
    }

    public void RemoveClient(Client client)
    {
        _clientRepo.Remove(client);
        _orderService.RemoveAllOrders(client.Id);
        Persist();
    }

    public Client? GetClientById(int id) => _clientRepo.GetById(id);
    public Client? GetByCPF(string CPF) => _clientRepo.GetByCPF(CPF);
    public List<Client> GetAllClients() => [.. _clientRepo.GetAll()];
    public List<Client> GetClientsByName(string name) => [.. _clientRepo.GetByName(name)];
    public void Persist() => _persistenceService.Save("clientes", _clientRepo.GetAll());
}
