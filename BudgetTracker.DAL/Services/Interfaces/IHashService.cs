using BudgetTracker.Core.Domain;

namespace BudgetTracker.DAL.Services.Interfaces
{
    public interface IHashService
    {
        public string HashPassword(string input);
        public bool VerifyPassword(string input, string hashedPassword);
        public string GenerateToken(Auth auth, int accessTokenExpirationTime);
        public RefreshToken GenerateRefreshToken(Guid userId, int refreshTokenExpirationTime);
        public string SignRefreshToken(RefreshToken refreshToken);
    }
}
