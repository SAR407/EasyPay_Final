using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByUserAsync(string username);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(System.DateTime start, System.DateTime end);
    }
}
