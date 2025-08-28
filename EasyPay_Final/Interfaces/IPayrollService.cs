using EasyPay_Final.Models;

namespace EasyPay_Final.Interfaces
{
    public interface IPayrollService
    {
        Task<Payroll> CalculatePayrollAsync(int employeeId, DateTime payrollDate);
        Task<bool> VerifyPayrollAsync(int payrollId);
        Task<bool> ProcessPaymentsAsync(int payrollId);
        Task<IEnumerable<Payroll>> GetPayrollHistoryAsync(int employeeId);
    }

}
