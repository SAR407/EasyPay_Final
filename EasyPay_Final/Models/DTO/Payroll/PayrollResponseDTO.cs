namespace EasyPay_Final.Models.DTO.Payroll
{
    public class PayrollResponseDTO
    {
        public int PayrollId { get; set; }
        public string? EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PayrollDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetSalary { get; set; }
        public bool IsProcessed { get; set; }
    }
}
