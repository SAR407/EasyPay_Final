using EasyPay_Final.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPay_Final.Repositories
{
    public interface ILeaveRequestRepository : IRepository<LeaveRequest>
    {
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetByStatusAsync(string status);
    }
}
