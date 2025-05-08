using BudgetTracker.BL.Services.Interfaces;
using BudgetTracker.Core.Domain;
using BudgetTracker.Core.Enums;
using BudgetTracker.DAL.DTOs.Auth;
using BudgetTracker.DAL.Services.Interfaces;

namespace BudgetTracker.BL.Services
{
    public class AuthService : IAuthService
    {
        protected readonly IUnitOfWork UOW;

        public AuthService(IUnitOfWork uow)
        {
            UOW = uow;
        }

        private async Task VerifyLoginIsUsed(string login, CancellationToken cancellationToken = default)
        {
            //var auth = await UOW.Auths.GetByLogin(login, cancellationToken);
            //if (auth is not null)
            //{
            //    if (!auth.Active || !auth.RegisterActivated)
            //    {
            //        //throw ValidationException<RegisterDTO>.CreateException(x => x.Login, ValidationError.LoginInactiveUserExists);
            //    }
            //    else
            //    {
            //        //throw ValidationException<RegisterDTO>.CreateException(x => x.Login, ValidationError.LoginInUse);
            //    }
            //}
        }

        private async Task VerifyEmailIsUsed(string email, CancellationToken cancellationToken = default)
        {
            //var auth = await UOW.Auths.GetByEmail(email, cancellationToken);
            //if (auth is not null)
            //{
            //    if (!auth.Active || !auth.RegisterActivated)
            //    {
            //        //throw ValidationException<RegisterDTO>.CreateException(x => x.Email, ValidationError.EmailInactiveUserExists);
            //    }
            //    else
            //    {
            //        //throw ValidationException<RegisterDTO>.CreateException(x => x.Email, ValidationError.EmailInUse);
            //    }
            //}
        }

        public Task<TokenDTO> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task RegisterAsync(RegisterDTO dto, CancellationToken cancellationToken = default)
        {
            var creationTime = DateTime.UtcNow;
            var registerGuid = Guid.NewGuid();

            if (dto.Password != dto.ConfirmPassword)
            {
                //throw ValidationException<RegisterDTO>.CreateException(x => x.Password, ValidationError.PasswordsDoNotMatch);
            }

            await VerifyLoginIsUsed(dto.Login, cancellationToken);
            await VerifyEmailIsUsed(dto.Email, cancellationToken);

            var user = new User()
            {
                Id = registerGuid,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            //UOW.Users.Create(user);

            var auth = new Auth()
            {
                Id = registerGuid,
                Email = dto.Email,
                Login = dto.Login,
                //Password = _hashService.HashPassword(dto.Password),
                AuthRole = AuthRole.User,
                RegisterGuid = Guid.NewGuid(),
                RegisterActivated = false,
                Active = false,
            };
            UOW.Auths.Create(auth);
            await UOW.SaveAsync(CancellationToken.None);


            throw new NotImplementedException();
        }
        public Task<TokenDTO> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task ActivateUserAsync(Guid registerGuid, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
