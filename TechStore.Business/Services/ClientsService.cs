using System;
using System.Collections.Generic;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Services.Authentication;

namespace TechStore.Business.Services;

public class ClientsService : IClientService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(
        IAuthenticationService authenticationService,
        IClientsRepository clientsRepository)
    {
        ArgumentNullException.ThrowIfNull(authenticationService);
        _authenticationService = authenticationService;
        
        ArgumentNullException.ThrowIfNull(clientsRepository);
        _clientsRepository = clientsRepository;
    }

    public Client RegisterClient(Client client)
    {
        var exists = _clientsRepository.ClientExistsByEmail(client.Email);
        if (exists) throw new InvalidModelException("Client with the same Email address exists already.");

        client.Password = _authenticationService.ComputePasswordHash(client.Password);
        return _clientsRepository.Insert(client);
    }

    public bool ClientExists(int clientId)
    {
        return _clientsRepository.ClientExistsById(clientId);
    }

    public IEnumerable<Client> GetAllClients()
    {
        return _clientsRepository.GetAll();
    }

    public Client GetClient(int clientId)
    {
        return _clientsRepository.Get(clientId);
    }

    public Client GetClientByEmail(string email)
    {
        return _clientsRepository.GetClientByEmail(email);
    }

    public void DeleteClient(string mail)
    {
        var client = _clientsRepository.GetClientByEmail(mail);
        var exists = client is not null;
        if (!exists) throw new UnknownModelException("Client", "Id", mail);

        _clientsRepository.Delete(client.Id);
    }
}