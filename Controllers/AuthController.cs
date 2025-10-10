using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ADOLab.Auth;

namespace ADOLab.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação e emissão de token JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtSettings _jwtSettings;

        /// <summary>Inicializa o controlador de autenticação.</summary>
        public AuthController(IConfiguration config, IOptions<JwtSettings> jwtOptions)
        {
            _config = config;
            _jwtSettings = jwtOptions.Value;
        }

        /// <summary>DTO de entrada para login.</summary>
        public record LoginRequest(string Username, string Password);

        /// <summary>DTO de resposta contendo o token e validade.</summary>
        public record LoginResponse(string Token, DateTime ExpiresAtUtc);

        /// <summary>
        /// Realiza login básico contra usuários de laboratório definidos no appsettings
        /// e emite um token JWT para uso nos demais endpoints.
        /// </summary>
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
        {
            try
            {
                // Busca usuários de laboratório no appsettings.json
                var users = _config.GetSection("Auth:Users").Get<List<Dictionary<string, string>>>() ?? new();

                // Valida credenciais
                var user = users.Find(u =>
                    u.TryGetValue("username", out var un) &&
                    u.TryGetValue("password", out var pw) &&
                    string.Equals(un, req.Username, StringComparison.OrdinalIgnoreCase) &&
                    pw == req.Password
                );

                if (user is null)
                    return Unauthorized(new { error = "Credenciais inválidas" });

                var role = user.TryGetValue("role", out var r) ? r : "User";

                // Monta claims
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, req.Username),
                    new Claim(ClaimTypes.Name, req.Username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Assina token
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
            catch (Exception ex)
            {
                // Retorna detalhe mínimo para diagnóstico em ambiente de laboratório
                return StatusCode(500, new { error = "Falha ao autenticar", detail = ex.Message });
            }
        }
    }
}
