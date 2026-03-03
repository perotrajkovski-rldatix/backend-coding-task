using System.Threading.Channels;

namespace Claims.Auditing
{
    public class InMemoryAuditQueue : IAuditQueue
    {
        private readonly Channel<AuditMessage> _channel;
        public InMemoryAuditQueue()
        {
            _channel = Channel.CreateUnbounded<AuditMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        public async ValueTask EnqueueAsync(AuditMessage message, CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public IAsyncEnumerable<AuditMessage> DequeueAllAsync(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
