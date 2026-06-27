using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventosVivos.API.Auth
{
    public class AuthService
    {
        private const string SecretKey = "EventosVivos_JWT_Key_2026#Reservas!";
        private const string Issuer = "EventosVivos.API";
        private const string Audience = "EventosVivos.Frontend";

        private readonly List<(string Username, string Password, string Role)> _usuarios = new()
        {
            ("admin", "Ev3nt0s@2026", "Admin"),
            ("user", "Us3r@Vivos!", "User")
        };

        public string? Login(string username, string password)
        {
            var usuario = _usuarios.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (usuario == default)
                return null;

            return GenerarToken(usuario.Username, usuario.Role);
        }

        private string GenerarToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static SymmetricSecurityKey GetKey() =>
            new(Encoding.UTF8.GetBytes(SecretKey));

        public static string GetIssuer() => Issuer;
        public static string GetAudience() => Audience;
    }
}
