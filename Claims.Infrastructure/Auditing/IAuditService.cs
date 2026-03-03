namespace Claims.Auditing;

public interface IAuditService
{
    ValueTask AuditClaimAsync(string claimId, string httpRequestType, CancellationToken cancellationToken = default);
    ValueTask AuditCoverAsync(string coverId, string httpRequestType, CancellationToken cancellationToken = default);
}