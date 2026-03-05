using System.Threading.Channels;
using Claims.Application.Abstractions.Auditing;

namespace Claims.Infrastructure.Auditing
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

        public async ValueTask EnqueueAsync(AuditMessage message)
        {
            await _channel.Writer.WriteAsync(message);
        }

        public IAsyncEnumerable<AuditMessage> DequeueAllAsync(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
