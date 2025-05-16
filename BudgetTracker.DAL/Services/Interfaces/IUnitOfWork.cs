using BudgetTracker.DAL.Repositories.Interfaces;

namespace BudgetTracker.DAL.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public IAuthRepository Auths { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        //public ICategoryRepository Categories { get; }
        //public IOperationRepository Operations { get; }

        // public IDbContextTransaction BeginTransaction();
        public Task SaveAsync(CancellationToken cancellationToken = default);


    }
}
