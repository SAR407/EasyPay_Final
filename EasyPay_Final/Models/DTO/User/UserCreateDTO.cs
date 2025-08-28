namespace EasyPay_Final.Models.DTO.User
{
    public class UserCreateDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
    }
}
