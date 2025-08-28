using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetByJoiningDateAsync(System.DateTime startDate, System.DateTime endDate);
    }
}
