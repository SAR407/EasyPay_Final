using Azure.Core;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Models;

using EasyPay_Final.Models.DTO.Authentication;

using EasyPay_Final.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyPay_Final.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticateService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<RegisterResponseDTO> RegisterUserAsync(RegisterRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.Username))
                throw new ArgumentException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(requestDTO.Password))
                throw new ArgumentException("Password cannot be empty.");

            // Check if username already exists
            var existingUser = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Username == requestDTO.Username);
            if (existingUser != null)
                throw new Exception("Username already exists.");

            var newUser = new User
            {
                Username = requestDTO.Username,
                Email = requestDTO.Email,
                RoleId = requestDTO.RoleId,
                IsActive = true,
                PasswordHash = HashPassword(requestDTO.Password)
            };

            await _userRepository.AddAsync(newUser);
           

            return new RegisterResponseDTO
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                
                RoleId = newUser.RoleId,
                IsActive = newUser.IsActive
            };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO)
        {
            var user = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Username == requestDTO.Username);

            if (user == null || !VerifyPassword(requestDTO.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
               
                Username = user.Username,
                RoleId = user.RoleId
            };
        }

        // Password hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
        //private bool VerifyPassword(string password, string storedHash)
        //{
        //    // If you saved plain text passwords in DB
        //    return password == storedHash;
        //}



        private string GenerateJwtToken(User user)
        {
            // Make sure role name is loaded
            var roleName = user.Role?.RoleName ?? user.RoleId.ToString();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName) // ✅ Use role name here
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
