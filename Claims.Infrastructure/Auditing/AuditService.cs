using Claims.Application.Abstractions.Auditing;

namespace Claims.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly IAuditQueue _auditQueue;

    public AuditService(IAuditQueue auditQueue)
    {
        _auditQueue = auditQueue;
    }

    public ValueTask AuditClaimAsync(string claimId, string httpRequestType)
    {
        var message = new AuditMessage(
            AuditEntityType.Claim,
            claimId,
            httpRequestType,
            DateTime.UtcNow);

        return _auditQueue.EnqueueAsync(message);
    }

    public ValueTask AuditCoverAsync(string coverId, string httpRequestType)
    {
        var message = new AuditMessage(
            AuditEntityType.Cover,
            coverId,
            httpRequestType,
            DateTime.UtcNow);

        return _auditQueue.EnqueueAsync(message);
    }
}