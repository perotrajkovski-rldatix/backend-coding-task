namespace Claims.Auditing;

public class AuditService : IAuditService
{
    private readonly IAuditQueue _auditQueue;

    public AuditService(IAuditQueue auditQueue)
    {
        _auditQueue = auditQueue;
    }

    public ValueTask AuditClaimAsync(string claimId, string httpRequestType, CancellationToken cancellationToken = default)
    {
        var message = new AuditMessage(
            AuditEntityType.Claim,
            claimId,
            httpRequestType,
            DateTime.UtcNow);

        return _auditQueue.EnqueueAsync(message, cancellationToken);
    }

    public ValueTask AuditCoverAsync(string coverId, string httpRequestType, CancellationToken cancellationToken = default)
    {
        var message = new AuditMessage(
            AuditEntityType.Cover,
            coverId,
            httpRequestType,
            DateTime.UtcNow);

        return _auditQueue.EnqueueAsync(message, cancellationToken);
    }
}