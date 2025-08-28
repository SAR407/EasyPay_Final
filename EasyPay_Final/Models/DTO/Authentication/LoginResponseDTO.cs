namespace EasyPay_Final.Models.DTO.Authentication
{
    public class LoginResponseDTO
    {
        public string? Token { get; set; }
        public int RoleId { get; set; }
        public string? Username { get; set; }
    }
}
