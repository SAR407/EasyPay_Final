using EasyPay_Final.Models;

namespace EasyPay_Final.Interfaces
{
    public interface IPayrollConfigService
    {
        Task ConfigurePayrollRulesAsync(PayrollPolicy config);
        Task<PayrollPolicy> GetPayrollConfigAsync();
    }

}
