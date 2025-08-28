namespace EasyPay_Final.Models.DTO.Timesheet
{
    public class TimesheetRequestDTO
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursWorked { get; set; }
        public string? TaskDescription { get; set; }
    }
}
