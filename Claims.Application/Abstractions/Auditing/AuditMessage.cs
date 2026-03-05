namespace Claims.Application.Abstractions.Auditing;

public sealed record AuditMessage(
    AuditEntityType EntityType,
    string EntityId,
    string HttpRequestType,
    DateTime CreatedUtc);
