namespace EasyPay_Final.Models
{
    public class PayrollPolicy
    {
        public int PayrollPolicyId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal HraPercent { get; set; }
        public decimal Allowances { get; set; }
        public decimal AllowancePercent { get; set; }
        public decimal Deductions { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }

}
