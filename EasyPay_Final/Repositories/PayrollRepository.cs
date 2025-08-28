using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface IPayrollRepository : IRepository<Payroll>
    {
        Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Payroll>> GetByDateRangeAsync(System.DateTime start, System.DateTime end);
    }
}
