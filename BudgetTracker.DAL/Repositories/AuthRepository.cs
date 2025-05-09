using BudgetTracker.Core;
using BudgetTracker.Core.Domain;
using BudgetTracker.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        protected readonly BudgetTrackerContext _context;
        protected readonly DbSet<Auth> _entities;

        public AuthRepository(BudgetTrackerContext context)
        {
            _context = context;
            _entities = context.Set<Auth>();
        }

        public void Create(Auth entity)
        {
            _entities.Add(entity);
        }

        public async Task<Auth?> GetByLogin(string login, CancellationToken cancellationToken)
        {
            var result = await _context.Auths.FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
            return result;
        }

        public async Task<Auth?> GetByRegisterGuid(Guid registerGuid, CancellationToken cancellationToken)
        {
            var result = await _context.Auths.FirstOrDefaultAsync(x => x.RegisterGuid == registerGuid, cancellationToken);
            return result;
        }

        public async Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken = default)
        {
            var result = await _context.Auths.AnyAsync(x => x.Email == email, cancellationToken);
            return result;
        }
    }
}
