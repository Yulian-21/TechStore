using System;

using Microsoft.Extensions.Configuration;

namespace TechStore.Business.Services.Content;

public interface IDiskContentStorageConfiguration
{
    string ContentStorageRoot { get; }
}

public class DiskContentStorageConfiguration : IDiskContentStorageConfiguration
{
    private readonly IConfiguration _configuration;
    
    public DiskContentStorageConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }
    
    public string ContentStorageRoot
        => _configuration.GetValue<string>("DiskContentStorage:Root");
}