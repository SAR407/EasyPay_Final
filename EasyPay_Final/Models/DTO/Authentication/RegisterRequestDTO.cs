namespace EasyPay_Final.Models.DTO.Authentication
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // Link to your roles table
    }
}
