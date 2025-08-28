using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class ComplianceReportRepositoryDB : RepositoryDB<ComplianceReport>, IComplianceReportRepository
    {
        public ComplianceReportRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<ComplianceReport>> GetAllAsync()
        {
            return await _context.ComplianceReports.ToListAsync();
        }

        public override async Task<ComplianceReport> GetByIdAsync(int id)
        {
            return await _context.ComplianceReports
                .FirstOrDefaultAsync(c => c.ReportId == id);
        }

        public async Task<IEnumerable<ComplianceReport>> GetByTypeAsync(string reportType)
        {
            return await _context.ComplianceReports
                .Where(c => c.ReportType == reportType)
                .ToListAsync();
        }
    }
}
