using BudgetTracker.Core;
using BudgetTracker.DAL.Repositories.Interfaces;
using BudgetTracker.DAL.Services.Interfaces;

namespace BudgetTracker.DAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly BudgetTrackerContext _context;

        public IUserRepository Users { get; }
        public IAuthRepository Auths { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        //public ICategoryRepository Categories { get; }
        //public IOperationRepository Operations { get; }

        public UnitOfWork(BudgetTrackerContext context, IUserRepository users, IAuthRepository auths, IRefreshTokenRepository refreshTokens)
        {
            _context = context;
            Users = users;
            Auths = auths;
            RefreshTokens = refreshTokens;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
