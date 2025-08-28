namespace EasyPay_Final.Models.DTO.Audit
{
    public class AuditLogResponseDTO
    {
        public int AuditLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Action { get; set; }
        public string? PerformedBy { get; set; }
        public string? Details { get; set; }
    }

}
