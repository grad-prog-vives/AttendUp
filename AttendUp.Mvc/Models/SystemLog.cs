namespace AttendUp.Mvc.Models
{
    public class SystemLog
    {
        // I define the primary key for the log entry
        public int Id { get; set; }

        // I store the level (e.g., Information, Warning, Error)
        public string LogLevel { get; set; } = string.Empty;

        // I store the actual log message or technical error details
        public string Message { get; set; } = string.Empty;

        // I record exactly when this event happend
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // I keep track of exception details if a technical crash happens
        public string? Exception { get; set; }

        // I store the username or email if the event is tied to a user session
        public string? Username { get; set; }

        // I capture the IP address to detect potential locations
        public string? IpAddress { get; set; }
    }
}

