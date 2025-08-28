using EasyPay_Final.Models;

namespace EasyPay_Final.Interfaces
{
    public interface ITimesheetService
    {
        Task<Timesheet> AddTimesheetEntryAsync(Timesheet timesheet);
        Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeAsync(int employeeId);
        Task<bool> ApproveTimesheetAsync(int timesheetId, int managerId);
    }

}
