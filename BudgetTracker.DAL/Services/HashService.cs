using BudgetTracker.Core.Domain;
using BudgetTracker.DAL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetTracker.DAL.Services
{
    public class HashService : IHashService
    {
        private readonly IConfiguration _config;

        public HashService(IConfiguration config)
        {
            _config = config;
        }

        public string HashPassword(string password)
        {

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return $"{Convert.ToBase64String(passwordSalt)}:{Convert.ToBase64String(passwordHash)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid hash format");
            var passwordSalt = Convert.FromBase64String(parts[0]);
            var passwordHash = Convert.FromBase64String(parts[1]);
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public string GenerateToken(Auth auth, int accessTokenExpirationTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("Auth:JwtToken").Value!);
            var authRole = (int)auth.AuthRole;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier, auth.Id.ToString()),
                    new(ClaimTypes.Role, authRole.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(accessTokenExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public RefreshToken GenerateRefreshToken(Guid userId, int refreshTokenExpirationMinutesOffset)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Expiration = DateTime.UtcNow.AddMinutes(refreshTokenExpirationMinutesOffset)
            };
            return refreshToken;
        }

        public string SignRefreshToken(RefreshToken refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("Auth:JwtRefreshToken").Value!);
            var tokenOptions = new JwtSecurityToken(
                claims: [
                    new Claim("refreshToken", refreshToken.Id.ToString())
                ],
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            string refreshTokenJWT = tokenHandler.WriteToken(tokenOptions);
            return refreshTokenJWT;
        }
    }
}
