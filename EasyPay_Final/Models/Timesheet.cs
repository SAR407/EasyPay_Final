namespace EasyPay_Final.Models
{
    public class Timesheet
    {
        public int TimesheetId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursWorked { get; set; }
        public string TaskDescription { get; set; } = string.Empty;

        // Status: Pending, Approved, Rejected
        public string Status { get; set; } = string.Empty;

        // FK
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

}
