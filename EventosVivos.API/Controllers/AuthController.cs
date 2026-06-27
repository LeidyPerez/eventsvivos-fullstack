using EventosVivos.API.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventosVivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var token = _authService.Login(dto.Username, dto.Password);

            if (token == null)
                return Unauthorized(new { error = "Usuario o contraseña incorrectos" });

            return Ok(new { token });
        }
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}