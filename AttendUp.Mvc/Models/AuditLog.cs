namespace AttendUp.Mvc.Models
{
    public class AuditLog
    {
        public int ID { get; set; }
        public string User { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
