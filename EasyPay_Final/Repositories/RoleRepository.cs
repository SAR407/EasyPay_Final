using EasyPay_Final.Models;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string roleName);
    }
}
