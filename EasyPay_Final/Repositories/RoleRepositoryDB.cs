using EasyPay_Final.Context;
using EasyPay_Final.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public class RoleRepositoryDB : RepositoryDB<Role>, IRoleRepository
    {
        public RoleRepositoryDB(EasypayDbContext context) : base(context) { }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.Users)
                .ToListAsync();
        }

        public override async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}
