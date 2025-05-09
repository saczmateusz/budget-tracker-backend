using BudgetTracker.DAL.Services.Interfaces;
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

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return $"{Convert.ToBase64String(passwordSalt)}:{Convert.ToBase64String(passwordHash)}";
            }
        }
    }
}
