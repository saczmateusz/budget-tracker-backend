using BudgetTracker.Core;
using BudgetTracker.Core.Domain;
using BudgetTracker.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly BudgetTrackerContext _context;
        protected readonly DbSet<User> _entities;

        public UserRepository(BudgetTrackerContext context)
        {
            _context = context;
            _entities = context.Set<User>();
        }

        public void Create(User entity)
        {
            _entities.Add(entity);
        }
    }
}
