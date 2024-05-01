using TechStore.Domain.Models.Users;

namespace TechStore.Domain.Services;

public interface IClientService
{
    Client RegisterClient(Client client);

    public IEnumerable<Client> GetAllClients();

    bool ClientExists(int clientId);

    void DeleteClient(string mail);
    
    Client GetClient(int clientId);

    Client GetClientByEmail(string email);
}