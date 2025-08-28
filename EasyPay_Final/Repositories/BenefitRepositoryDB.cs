using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class BenefitRepositoryDB : RepositoryDB<Benefit>, IBenefitRepository
    {
        public BenefitRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<Benefit>> GetAllAsync()
        {
            return await _context.Benefits
                .Include(b => b.Employee)
                .ThenInclude(e => e.User)
                .ToListAsync();
        }

        public override async Task<Benefit> GetByIdAsync(int id)
        {
            return await _context.Benefits
                .Include(b => b.Employee)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(b => b.BenefitId == id);
        }

        public async Task<IEnumerable<Benefit>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Benefits
                .Where(b => b.EmployeeId == employeeId)
                .Include(b => b.Employee)
                .ToListAsync();
        }
    }
}
