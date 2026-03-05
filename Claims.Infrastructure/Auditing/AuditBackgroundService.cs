using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Claims.Application.Abstractions.Auditing;

namespace Claims.Infrastructure.Auditing;

public class AuditBackgroundService : BackgroundService
{
    private readonly IAuditQueue _auditQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuditBackgroundService> _logger;

    public AuditBackgroundService(
        IAuditQueue auditQueue,
        IServiceScopeFactory scopeFactory,
        ILogger<AuditBackgroundService> logger)
    {
        _auditQueue = auditQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _auditQueue.DequeueAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var auditContext = scope.ServiceProvider.GetRequiredService<AuditContext>();

                if (message.EntityType == AuditEntityType.Claim)
                {
                    auditContext.ClaimAudits.Add(new ClaimAudit
                    {
                        ClaimId = message.EntityId,
                        HttpRequestType = message.HttpRequestType,
                        Created = message.CreatedUtc
                    });
                }
                else
                {
                    auditContext.CoverAudits.Add(new CoverAudit
                    {
                        CoverId = message.EntityId,
                        HttpRequestType = message.HttpRequestType,
                        Created = message.CreatedUtc
                    });
                }

                await auditContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist audit message for entity {EntityId}", message.EntityId);
            }
        }
    }
}
