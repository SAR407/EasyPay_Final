namespace EasyPay_Final.Models.DTO.LeaveRequest
{
    public class LeaveRequestDTO
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
