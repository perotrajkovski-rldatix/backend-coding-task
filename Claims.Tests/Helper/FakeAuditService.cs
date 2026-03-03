using Claims.Auditing;

namespace Claims.Tests.Helper;

public sealed class FakeAuditService : IAuditService
{
    public ValueTask AuditClaimAsync(string claimId, string httpRequestType)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask AuditCoverAsync(string coverId, string httpRequestType)
    {
        return ValueTask.CompletedTask;
    }
}