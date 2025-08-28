using EasyPay_Final.Interfaces;
using EasyPay_Final.Models.DTO.Authentication;


using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EasyPay_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticate _authenticateService;

        public AuthenticationController(IAuthenticate authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDTO requestDTO)
        {
            try
            {
                var result = await _authenticateService.RegisterUserAsync(requestDTO);
                return Created("", result);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new ErrorObjectDTO
                {
                    ErrorNumber = 430,
                    ErrorMessage = message
                });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            try
            {
                var result = await _authenticateService.LoginAsync(requestDTO);
                return Ok(result);
            }
            catch (Exception)
            {
                return Unauthorized(new ErrorObjectDTO
                {
                    ErrorNumber = 401,
                    ErrorMessage = "Invalid username or password"
                });
            }
        }
    }
}
