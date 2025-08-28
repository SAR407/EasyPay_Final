namespace EasyPay_Final.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

}
