namespace EasyPay_Final.Models
{
    public class Benefit
    {
        public int BenefitId { get; set; }
        public string BenefitType { get; set; } = string.Empty; // Health Insurance, PF, Bonus, etc.
        public decimal Amount { get; set; }

        // FK
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

}
