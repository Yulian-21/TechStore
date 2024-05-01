namespace TechStore.Domain.Services;

public interface IContentStorageService
{
    Stream GetContent(string storageKey);

    string StoreContent(Stream stream);

    void RemoveContent(string storageKey);
}