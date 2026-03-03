namespace Claims.Auditing;

public interface IAuditQueue
{
    ValueTask EnqueueAsync(AuditMessage message);
    IAsyncEnumerable<AuditMessage> DequeueAllAsync(CancellationToken cancellationToken = default);
}