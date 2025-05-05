using BudgetTracker.BL.Interfaces;
using BudgetTracker.DAL.DTOs.Auth;

namespace BudgetTracker.BL.Services
{
    public class AuthService : IAuthService
    {
        public Task<TokenDTO> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task RegisterAsync(RegisterDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<TokenDTO> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task ActivateUserAsync(Guid registerGuid, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
