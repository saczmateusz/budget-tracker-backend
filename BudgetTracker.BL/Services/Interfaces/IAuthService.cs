using BudgetTracker.DAL.DTOs.Auth;

namespace BudgetTracker.BL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default);
        Task RegisterAsync(RegisterDTO dto, CancellationToken cancellationToken = default);
        Task<TokenDTO> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task ActivateUserAsync(Guid registerGuid, CancellationToken cancellationToken = default);
    }
}
