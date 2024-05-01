using System;
using System.IO;
using System.IO.Abstractions;

using TechStore.Domain.Services;

namespace TechStore.Business.Services.Content;

public class DiskContentStorageService : IContentStorageService
{
    private readonly IFileSystem _fileSystem;
    private readonly IDiskContentStorageConfiguration _configuration;
    
    public DiskContentStorageService(
        IFileSystem fileSystem,
        IDiskContentStorageConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(fileSystem);
        _fileSystem = fileSystem;
        
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }
    
    public Stream GetContent(string storageKey)
    {
        var path = Path.Combine(_configuration.ContentStorageRoot, storageKey);
        if (!_fileSystem.File.Exists(path)) return null;

        return _fileSystem.File.OpenRead(path);
    }

    public string StoreContent(Stream stream)
    {
        _fileSystem.Directory.CreateDirectory(_configuration.ContentStorageRoot);
        
        var storageKey = Guid.NewGuid().ToString();
        var path = Path.Combine(_configuration.ContentStorageRoot, storageKey);
        
        using var destination = _fileSystem.File.Create(path);
        stream.CopyTo(destination);
        destination.Close();

        return storageKey;
    }

    public void RemoveContent(string storageKey)
    {
        var path = Path.Combine(_configuration.ContentStorageRoot, storageKey);
        if (_fileSystem.File.Exists(path)) _fileSystem.File.Delete(path);
    }
}