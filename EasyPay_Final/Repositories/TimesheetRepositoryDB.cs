using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class TimesheetRepositoryDB : RepositoryDB<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<Timesheet>> GetAllAsync()
        {
            return await _context.Timesheets
                .Include(t => t.Employee)
                .ThenInclude(e => e.User)
                .ToListAsync();
        }

        public override async Task<Timesheet> GetByIdAsync(int id)
        {
            return await _context.Timesheets
                .Include(t => t.Employee)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(t => t.TimesheetId == id);
        }

        public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Timesheets
                .Where(t => t.EmployeeId == employeeId)
                .Include(t => t.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Timesheet>> GetByDateRangeAsync(System.DateTime start, System.DateTime end)
        {
            return await _context.Timesheets
                .Where(t => t.Date >= start && t.Date <= end)
                .Include(t => t.Employee)
                .ToListAsync();
        }
    }
}
