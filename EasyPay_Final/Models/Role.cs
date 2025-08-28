namespace EasyPay_Final.Models
{
    public class Role
    {
        public int RoleId { get; set; }         // PK
        public string RoleName { get; set; } = string.Empty;   // Admin, PayrollProcessor, Employee, Manager

        // Navigation
        public ICollection<User>? Users { get; set; }
    }

}
