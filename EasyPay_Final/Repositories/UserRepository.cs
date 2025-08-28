using EasyPay_Final.Models;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
    }
}
