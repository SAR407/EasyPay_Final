namespace EasyPay_Final.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } // Store hashed passwords
        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        // Navigation
        public Employee? Employee { get; set; }  // If this user is linked to an employee
    }
}
