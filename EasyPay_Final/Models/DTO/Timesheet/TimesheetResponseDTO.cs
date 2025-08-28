namespace EasyPay_Final.Models.DTO.Timesheet
{
    public class TimesheetResponseDTO
    {
        public int TimesheetId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Status { get; set; }
    }
}
