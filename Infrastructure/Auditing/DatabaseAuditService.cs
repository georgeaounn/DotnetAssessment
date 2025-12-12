using Application.Abstractions.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Auditing
{
    public class DatabaseAuditService : IAuditService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DatabaseAuditService> _logger;

        public DatabaseAuditService(AppDbContext dbContext, ILogger<DatabaseAuditService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task RecordAsync(string action, string entityName, string entityId, Guid? userId, CancellationToken ct)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    Action = action,
                    EntityName = entityName,
                    EntityId = entityId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.AuditLogs.AddAsync(auditLog, ct);
                await _dbContext.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Failed to record audit log. Action: {Action}, Entity: {EntityName}, EntityId: {EntityId}, UserId: {UserId}",
                    action, entityName, entityId, userId);
            }
        }
    }
}
