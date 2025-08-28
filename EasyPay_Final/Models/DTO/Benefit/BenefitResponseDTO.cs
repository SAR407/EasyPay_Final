namespace EasyPay_Final.Models.DTO.Benefit
{
    public class BenefitResponseDTO
    {
        public int BenefitId { get; set; }
        public string? EmployeeName { get; set; }
        public string? BenefitType { get; set; }
        public decimal Amount { get; set; }
    }
}
