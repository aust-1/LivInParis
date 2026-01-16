using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using LivInParisRoussilleTeynier.Domain.Models.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LivInParisRoussilleTeynier.Services.Services
{
    public class TokenService : ITokenService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int HashIterations = 100_000;

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
            // Ici un simple GUID, en prod on stocke en base
            return Guid.NewGuid().ToString("N");
        }

        public string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                HashIterations,
                HashAlgorithmName.SHA256,
                HashSize
            );
            return $"{HashIterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length
            );

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }


        public Task RevokeTokenAsync(string refreshToken)
        {
            // implémentation stub
            return Task.CompletedTask;
        }
    }
}
