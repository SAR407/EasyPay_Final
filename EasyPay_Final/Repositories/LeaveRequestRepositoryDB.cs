using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class LeaveRequestRepositoryDB : RepositoryDB<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .ThenInclude(e => e.User)
                .ToListAsync();
        }

        public override async Task<LeaveRequest> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(l => l.LeaveRequestId == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByStatusAsync(string status)
        {
            return await _context.LeaveRequests
                .Where(l => l.Status == status)
                .Include(l => l.Employee)
                .ToListAsync();
        }
    }
}
