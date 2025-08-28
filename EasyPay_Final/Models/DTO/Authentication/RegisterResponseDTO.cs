namespace EasyPay_Final.Models.DTO.Authentication
{
    public class RegisterResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        
        public bool IsActive { get; set; }
    }
}
