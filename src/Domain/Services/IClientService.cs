using Domain.Entities;

namespace Domain.Services;

public interface IClientService
{
    void AddClient(Client client);
    List<Client> GetAllClients();
    Client? GetByCPF(string CPF);
    Client? GetClientById(int id);
    List<Client> GetClientsByName(string name);
    void RemoveClient(Client client);
    void Persist();
}