using Claims.Application.Abstractions;

namespace Claims.Tests.Helper;

public sealed class FakeCoverRepository : ICoverRepository
{
    public Cover? CoverToReturn { get; set; }
    public bool CreateCalled { get; private set; }

    public Task<IReadOnlyList<Cover>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyList<Cover>)Array.Empty<Cover>());
    }

    public Task<Cover?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CoverToReturn);
    }

    public Task CreateAsync(Cover cover, CancellationToken cancellationToken = default)
    {
        CreateCalled = true;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}