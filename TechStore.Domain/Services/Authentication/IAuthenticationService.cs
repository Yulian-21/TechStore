using TechStore.Domain.Models.Users;

namespace TechStore.Domain.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult ClientLogin(string email, string password);

    AuthenticationResult AdminLogin(string email, string password);

    string ComputePasswordHash(string password);
}
