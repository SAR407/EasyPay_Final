namespace EasyPay_Final.Models.DTO.Benefit
{
    public class BenefitRequestDTO
    {
        public int EmployeeId { get; set; }
        public string? BenefitType { get; set; }
        public decimal Amount { get; set; }
    }
}
