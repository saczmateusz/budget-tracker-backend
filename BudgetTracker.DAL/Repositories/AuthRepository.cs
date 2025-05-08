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
    }
}
