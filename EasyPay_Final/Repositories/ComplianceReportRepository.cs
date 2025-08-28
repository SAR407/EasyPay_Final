using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IComplianceReportRepository : IRepository<ComplianceReport>
    {
        Task<IEnumerable<ComplianceReport>> GetByTypeAsync(string reportType);
    }
}
