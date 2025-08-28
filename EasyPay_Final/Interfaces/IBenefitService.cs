using EasyPay_Final.Models;

namespace EasyPay_Final.Interfaces
{
    public interface IBenefitService
    {
        Task<Benefit> AddBenefitAsync(Benefit benefit);
        Task<IEnumerable<Benefit>> GetBenefitsByEmployeeAsync(int employeeId);
        Task<bool> UpdateBenefitAsync(int benefitId, Benefit benefit);
    }

}
