using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LivInParisRoussilleTeynier.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = _config["JwtSettings:Key"];
            _issuer = _config["JwtSettings:Issuer"];
            _audience = _config["JwtSettings:Audience"];
            _expiryMinutes = int.Parse(_config["JwtSettings:ExpiryMinutes"]);
        }

        public string GenerateToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                        new Claim(ClaimTypes.Name, user.AccountUserName),
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDesc));
        }

        public string GenerateRefreshToken(Account user)
        {
            return Guid.NewGuid().ToString("N");
        }

        public string HashPassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        public Task RevokeTokenAsync(string refreshToken)
        {
            return Task.CompletedTask;
        }
    }
}
