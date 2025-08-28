using EasyPay_Final.Models;

namespace EasyPay_Final.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveRequest> SubmitLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeAsync(int employeeId);
        Task<bool> ApproveLeaveAsync(int leaveRequestId, int managerId);
        Task<bool> RejectLeaveAsync(int leaveRequestId, int managerId);
    }

}
