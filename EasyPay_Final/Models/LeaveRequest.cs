namespace EasyPay_Final.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected

        // FK
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

}
