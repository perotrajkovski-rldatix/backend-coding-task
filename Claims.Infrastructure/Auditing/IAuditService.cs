namespace Claims.Auditing;

public interface IAuditService
{
    ValueTask AuditClaimAsync(string claimId, string httpRequestType);
    ValueTask AuditCoverAsync(string coverId, string httpRequestType);
}