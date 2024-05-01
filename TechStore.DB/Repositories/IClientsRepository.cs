using TechStore.Domain.Models.Users;

namespace TechStore.DB.Repositories;

public interface IClientsRepository : IDataRepository<Client>
{
    bool ClientExistsById(int clientId);
    
    bool ClientExistsByEmail(string email);

    Client GetClientByEmail(string email);
}