using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ADOLab.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IConfiguration config, IOptions<JwtSettings> jwtOptions)
        {
            _config = config;
            _jwtSettings = jwtOptions.Value;
        }

        public record LoginRequest(string Username, string Password);
        public record LoginResponse(string Token, DateTime ExpiresAtUtc);

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
        {
            var users = _config.GetSection("Auth:Users").Get<List<Dictionary<string, string>>>() ?? new();
            var user = users.Find(u =>
                u.TryGetValue("username", out var un) &&
                u.TryGetValue("password", out var pw) &&
                string.Equals(un, req.Username, StringComparison.OrdinalIgnoreCase) &&
                pw == req.Password
            );
            if (user is null) return Unauthorized(new { error = "Credenciais inválidas" });
            var role = user.TryGetValue("role", out var r) ? r : "User";

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, req.Username),
                new Claim(ClaimTypes.Name, req.Username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResponse(jwt, expires));
        }
    }
}