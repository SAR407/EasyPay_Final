using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface ITimesheetRepository : IRepository<Timesheet>
    {
        Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Timesheet>> GetByDateRangeAsync(System.DateTime start, System.DateTime end);
    }
}
