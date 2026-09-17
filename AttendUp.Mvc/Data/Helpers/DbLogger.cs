using AttendUp.Mvc.Models;

namespace AttendUp.Mvc.Data.Helpers
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public DbLoggerProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        // I create and return the actual logger instance
        public ILogger CreateLogger(string categoryName) => new DbLogger(categoryName, _serviceProvider);
        public void Dispose() { }
    }

    public class DbLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IServiceProvider _serviceProvider;

        public DbLogger(string categoryName, IServiceProvider serviceProvider)
        {
            _categoryName = categoryName;
            _serviceProvider = serviceProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning; // I only persist Warnings and Errors to save DB space

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            // CRITICAL: I ignore EF Core database logging to prevent an infinite logging loop!
            if (_categoryName.StartsWith("Microsoft.EntityFrameworkCore")) return;

            var message = formatter(state, exception);

            // I create a separate scope to safely resolve the AttendUpContext from the DI container
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AttendUpContext>();
            var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor?.HttpContext;

            var logEntry = new SystemLog
            {
                LogLevel = logLevel.ToString(),
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Exception = exception?.ToString(),
                Username = httpContext?.User?.Identity?.Name,
                IpAddress = GetClientIpAddress(httpContext) // Use the safe IP resolution logic
            };

            // I save the log entry directly into the SQL database
            context.SystemLogs.Add(logEntry);
            context.SaveChanges();
        }

        private string? GetClientIpAddress(HttpContext? context)
        {
            if (context == null) return null;

            // 1. Check for reverse proxies (IIS, Nginx, Cloudflare)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor) && !string.IsNullOrEmpty(forwardedFor))
            {
                // X-Forwarded-For can contain a comma-separated list (client, proxy1, proxy2). Always pick the first one.
                var firstIp = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();

                if (firstIp == "::1") return "127.0.0.1";
                return firstIp;
            }

            // 2. No proxy? Get the direct remote IP address
            var remoteIp = context.Connection?.RemoteIpAddress;
            if (remoteIp == null) return null;

            // If it is an IPv6 or IPv4 loopback/localhost address, enforce 127.0.0.1
            if (System.Net.IPAddress.IsLoopback(remoteIp))
            {
                return "127.0.0.1";
            }

            // Map any IPv4-mapped IPv6 addresses cleanly to an IPv4 string representation
            return remoteIp.IsIPv4MappedToIPv6 ? remoteIp.MapToIPv4().ToString() : remoteIp.ToString();
        }
    }
}