using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IBenefitRepository : IRepository<Benefit>
    {
        Task<IEnumerable<Benefit>> GetByEmployeeIdAsync(int employeeId);
    }
}
