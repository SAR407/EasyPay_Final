using EasyPay_Final.Models;

public interface IAuditService
{
    Task LogActionAsync(string performedBy, string action, string details);
    Task<IEnumerable<AuditLog>> GetAuditLogsAsync();
}
