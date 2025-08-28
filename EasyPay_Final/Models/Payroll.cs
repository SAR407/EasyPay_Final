namespace EasyPay_Final.Models
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public DateTime PayrollDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetSalary { get; set; }
        public bool IsProcessed { get; set; }

        // FK
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

}
