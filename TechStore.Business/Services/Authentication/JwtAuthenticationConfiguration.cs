using System;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace TechStore.Business.Services.Authentication;

public interface IJwtAuthenticationConfiguration
{
    byte[] Secret { get; }

    int ExpireHours { get; }
}

public class JwtAuthenticationConfiguration : IJwtAuthenticationConfiguration
{
    private readonly IConfiguration _configuration;

    public JwtAuthenticationConfiguration(
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }
    
    public byte[] Secret
    {
        get
        {
            var section = _configuration.GetSection("Authentication:Jwt");
            return Encoding.ASCII.GetBytes(section["Secret"]);
        }
    }

    public int ExpireHours
    {
        get
        {
            var section = _configuration.GetSection("Authentication:Jwt");
            return section.GetValue<int>("ExpireHours");
        }
    }
}