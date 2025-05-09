using BudgetTracker.Core.Domain;

namespace BudgetTracker.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public void Create(User entity);

        //public void Update(User entity);

        //public void Delete(Guid id);
        //public void Delete(User entityToDelete);

        //public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //public Task<User?> GetByLogin(string login, CancellationToken cancellationToken = default);
    }
}
