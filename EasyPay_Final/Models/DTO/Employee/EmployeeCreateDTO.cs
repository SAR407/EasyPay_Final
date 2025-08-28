namespace EasyPay_Final.Models.DTO.Employee
{
    public class EmployeeCreateDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string ?Email { get; set; }
        public string? Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public int UserId { get; set; }
    }
}
