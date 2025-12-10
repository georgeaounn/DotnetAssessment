using Application.Abstractions.Services;

namespace Infrastructure.Logging
{

    public class FileErrorLogger : IErrorLogger
    {
        private readonly string _path;

        public FileErrorLogger()
        {
            _path = Path.Combine(AppContext.BaseDirectory, "logs", "errors.log");
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        }

        public Task LogErrorAsync(Exception ex, string? context, CancellationToken ct = default)
        {
            var line = $"{DateTime.UtcNow:o} | {context} | {ex}\n";
            return File.AppendAllTextAsync(_path, line, ct);
        }
    }
}