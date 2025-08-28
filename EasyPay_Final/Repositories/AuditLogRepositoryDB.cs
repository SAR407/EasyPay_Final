using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class AuditLogRepositoryDB : RepositoryDB<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .ToListAsync();
        }
        //public override async Task<AuditLog> AddAsync(AuditLog entity)
        //{
        //    await _context.AuditLogs.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //    return entity;
        //}

        public override async Task<AuditLog> GetByIdAsync(int id)
        {
            return await _context.AuditLogs
                .FirstOrDefaultAsync(a => a.AuditLogId == id);
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string username)
        {
            return await _context.AuditLogs
                .Where(a => a.PerformedBy == username)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(System.DateTime start, System.DateTime end)
        {
            return await _context.AuditLogs
                .Where(a => a.Timestamp >= start && a.Timestamp <= end)
                .ToListAsync();
        }
    }
}
