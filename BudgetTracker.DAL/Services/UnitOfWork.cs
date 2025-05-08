using BudgetTracker.Core;
using BudgetTracker.DAL.Repositories.Interfaces;
using BudgetTracker.DAL.Services.Interfaces;

namespace BudgetTracker.DAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly BudgetTrackerContext _context;

        //public IUserRepository Users { get; }
        public IAuthRepository Auths { get; }
        //public ICategoryRepository Categories { get; }
        //public IOperationRepository Operations { get; }

        public UnitOfWork(BudgetTrackerContext context, IAuthRepository auths)
        {
            _context = context;
            Auths = auths;
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
