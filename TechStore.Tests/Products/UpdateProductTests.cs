using Xunit;

namespace TechStore.Tests.Products;

[Collection(nameof(TechStoreApiTests))]
public class UpdateProductTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public UpdateProductTests(TechStoreApiFixture api)
    {
        _api = api;
    }
}