namespace EasyPay_Final.Models.DTO.Employee
{
    public class EmployeeResponseDTO
    {
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
    }
}
