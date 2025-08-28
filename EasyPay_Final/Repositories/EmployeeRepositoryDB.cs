using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class EmployeeRepositoryDB : RepositoryDB<Employee>, IEmployeeRepository
    {
        public EmployeeRepositoryDB(EasypayDbContext context) : base(context) { }

        // Explicitly implement GetAll as you requested
        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Payrolls)
                .Include(e => e.LeaveRequests)
                .Include(e => e.Timesheets)
                .Include(e => e.Benefits)
                .ToListAsync();
        }

        public override async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Payrolls)
                .Include(e => e.LeaveRequests)
                .Include(e => e.Timesheets)
                .Include(e => e.Benefits)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetByJoiningDateAsync(System.DateTime startDate, System.DateTime endDate)
        {
            return await _context.Employees
                .Where(e => e.JoiningDate >= startDate && e.JoiningDate <= endDate)
                .Include(e => e.User)
                .ToListAsync();
        }
    }
}
