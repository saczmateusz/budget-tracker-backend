using BudgetTracker.Core.Domain;

namespace BudgetTracker.DAL.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public void Create(Auth entity);

        //public void Update(Auth entity);

        //public void Delete(Guid id);
        //public void Delete(Auth entityToDelete);

        //public Task<Auth?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        //public Task<Auth?> GetByLogin(string login, CancellationToken cancellationToken = default);
        //public Task<bool> IsLoginInUse(string login, CancellationToken cancellationToken = default);
        //public Task<bool> IsEmailInUse(Guid accountId, string email, CancellationToken cancellationToken = default);
    }
}
