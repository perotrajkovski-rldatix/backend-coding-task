namespace Claims.Auditing;

public interface IAuditQueue
{
    ValueTask EnqueueAsync(AuditMessage message, CancellationToken cancellationToken = default);
    IAsyncEnumerable<AuditMessage> DequeueAllAsync(CancellationToken cancellationToken = default);
}