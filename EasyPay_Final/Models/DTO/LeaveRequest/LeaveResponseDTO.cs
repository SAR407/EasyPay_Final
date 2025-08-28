namespace EasyPay_Final.Models.DTO.LeaveRequest
{
    public class LeaveResponseDTO
    {
        public int LeaveRequestId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
    }
}
