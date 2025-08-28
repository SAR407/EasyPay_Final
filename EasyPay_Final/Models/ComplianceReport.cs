namespace EasyPay_Final.Models
{
    public class ComplianceReport
    {
        public int ReportId { get; set; }
        public string ReportType { get; set; } = string.Empty; // Tax, Statutory, Other
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime GeneratedOn { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;// Path or URL to the generated file
    }

}
