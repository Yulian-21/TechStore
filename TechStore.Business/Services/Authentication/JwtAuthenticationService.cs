using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;

using TechStore.Domain.Models.Users;
using TechStore.Domain.Services.Authentication;
using TechStore.Domain.Types;

namespace TechStore.Business.Services.Authentication;

public class JwtAuthenticationService : IAuthenticationService
{
    private readonly IJwtAuthenticationConfiguration _configuration;
    private readonly IClientsRepository _clientsRepository;
    private readonly IAdminsRepository _adminsRepository;

    public JwtAuthenticationService(
        IJwtAuthenticationConfiguration configuration,
        IClientsRepository clientsRepository,
        IAdminsRepository adminsRepository)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
        
        ArgumentNullException.ThrowIfNull(clientsRepository);
        _clientsRepository = clientsRepository;

        ArgumentNullException.ThrowIfNull(adminsRepository);
        _adminsRepository = adminsRepository;
    }

    public AuthenticationResult ClientLogin(string email, string password)
    {
        var client = _clientsRepository.GetClientByEmail(email);
        if (client is null) throw new UnknownModelException("Client", "Email", email);

        var hash = ComputePasswordHash(password);
        if (hash != client.Password) throw new AuthenticationFailedException(email);

        return GenerateToken(email);
    }

    public AuthenticationResult AdminLogin(string email, string password)
    {
        var admin = _adminsRepository.GetAdminByEmail(email);
        if (admin is null) throw new UnknownModelException("Admin", "Email", email);

        var hash = ComputePasswordHash(password);
        if (hash != admin.Password) throw new AuthenticationFailedException(email);

        return GenerateToken(email);
    }

    public string ComputePasswordHash(string password)
    {
        var bytes = Encoding.ASCII.GetBytes(password);
        
        using var algorithm = SHA256.Create();
        var hash = algorithm.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    private List<string> GetUserRoles(string email)
    {
        var admin = new Admin();
        var client = _clientsRepository.GetClientByEmail(email);
        if (client is null) admin = _adminsRepository.GetAdminByEmail(email);

        if (client != null)
            return new List<string> { UserRole.Client };
        else if (admin != null)
            return new List<string> { UserRole.Admin };
        return new List<string>();
    }

    private AuthenticationResult GenerateToken(string email)
    {
        var expireHours = _configuration.ExpireHours;
        var expires = DateTime.UtcNow.AddHours(expireHours);

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var securityTokenDescriptor = GenerateSecurityTokenDescriptor(email, expires);

        return new AuthenticationResult
        {
            Username = email,
            Token = securityTokenHandler.WriteToken(securityTokenHandler.CreateToken(securityTokenDescriptor)),
            ExpiresUtc = expires,
        };
    }

    private SecurityTokenDescriptor GenerateSecurityTokenDescriptor(string email, in DateTime expires)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, email)
        };

        var roles = GetUserRoles(email);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_configuration.Secret),
                SecurityAlgorithms.HmacSha256Signature),
        };
    }
}
