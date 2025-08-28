using EasyPay_Final.Models;
namespace EasyPay_Final.Interfaces
{
    public interface IComplianceService
    {
        Task<IEnumerable<ComplianceReport>> GenerateTaxReportsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ComplianceReport>> GenerateStatutoryReportsAsync();
    }

}
