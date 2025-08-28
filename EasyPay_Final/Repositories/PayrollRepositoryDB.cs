using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class PayrollRepositoryDB : RepositoryDB<Payroll>, IPayrollRepository
    {
        public PayrollRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<Payroll>> GetAllAsync()
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .ThenInclude(e => e.User)
                .ToListAsync();
        }

        public override async Task<Payroll> GetByIdAsync(int id)
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(p => p.PayrollId == id);
        }

        public async Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .Include(p => p.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payroll>> GetByDateRangeAsync(System.DateTime start, System.DateTime end)
        {
            return await _context.Payrolls
                .Where(p => p.PayrollDate >= start && p.PayrollDate <= end)
                .Include(p => p.Employee)
                .ToListAsync();
        }
    }
}
