using BudgetTracker.Core.Domain;

namespace BudgetTracker.DAL.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public void Create(RefreshToken entity);

        public bool Attach(RefreshToken entity);

        //public void Delete(Guid id);
        //public void Delete(RefreshToken entityToDelete);

        public Task<RefreshToken?> GetUserLatestRefreshToken(Guid userId, CancellationToken cancellationToken = default);
    }
}
