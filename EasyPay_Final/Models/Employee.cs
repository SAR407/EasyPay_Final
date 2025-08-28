namespace EasyPay_Final.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
       
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }

        // FK to User (login)
        public int UserId { get; set; }
        public User? User { get; set; }

        // Navigation
        public ICollection<Payroll>? Payrolls { get; set; }
        public ICollection<LeaveRequest>? LeaveRequests { get; set; }
        public ICollection<Timesheet> Timesheets { get; set; }
        public ICollection<Benefit> Benefits { get; set; }


    }

}
