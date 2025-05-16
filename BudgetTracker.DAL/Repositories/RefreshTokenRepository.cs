using BudgetTracker.Core.Domain;
using BudgetTracker.Core;
using BudgetTracker.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.DAL.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        protected readonly BudgetTrackerContext _context;
        protected readonly DbSet<RefreshToken> _entities;

        public RefreshTokenRepository(BudgetTrackerContext context)
        {
            _context = context;
            _entities = context.Set<RefreshToken>();
        }

        public void Create(RefreshToken entity)
        {
            _entities.Add(entity);
        }

        // ----------------------------------------------

        public bool Attach(RefreshToken entity)
        {
            var attached = false;
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
                attached = true;
            }
            return attached;
        }

        public async Task<RefreshToken?> GetUserLatestRefreshToken(Guid userId, CancellationToken cancellationToken = default)
        {
            var dateUtcNow = DateTime.UtcNow;
            var result = await _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Expiration > dateUtcNow)
                .OrderByDescending(x => x.Expiration)
                .FirstOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
