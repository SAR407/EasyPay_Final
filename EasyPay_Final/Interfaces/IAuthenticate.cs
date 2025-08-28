
    using EasyPay_Final.Models.DTO.Authentication;
   
    
    
    using System.Threading.Tasks;

    namespace EasyPay_Final.Interfaces
    {
        public interface IAuthenticate
        {
            /// <summary>
            /// Registers a new user (Employee, Admin, etc.)
            /// </summary>
            Task<RegisterResponseDTO> RegisterUserAsync(RegisterRequestDTO requestDTO);

            /// <summary>
            /// Authenticates user and returns a JWT token + basic user info
            /// </summary>
            Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO);
        }
    }


