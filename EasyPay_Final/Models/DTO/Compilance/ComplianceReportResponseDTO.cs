namespace EasyPay_Final.Models.DTO.Compilance
{
    public class ComplianceReportResponseDTO
    {
        public int ReportId { get; set; }
        public string? ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime GeneratedOn { get; set; }
        public string? GeneratedBy { get; set; }
        public string? FilePath { get; set; }
    }

}
