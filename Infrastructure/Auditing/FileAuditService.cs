
using Application.Abstractions.Services;

namespace Infrastructure.Auditing
{
    public class FileAuditService : IAuditService
    {
        private readonly string _path;

        public FileAuditService()
        {
            _path = Path.Combine(AppContext.BaseDirectory, "logs", "audit.log");
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        }

        public Task RecordAsync(string action, string entityName, string entityId, Guid? userId, CancellationToken ct)
        {
            var line = $"{DateTime.UtcNow:o} | {action} | {entityName}:{entityId} | User:{userId}\n";
            return File.AppendAllTextAsync(_path, line, ct);
        }
    }
}