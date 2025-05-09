using BudgetTracker.BL.Services.Interfaces;
using BudgetTracker.Core.Domain;
using BudgetTracker.Core.Enums;
using BudgetTracker.DAL.DTOs.Auth;
using BudgetTracker.DAL.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.BL.Services
{
    public class AuthService : IAuthService
    {
        protected readonly IUnitOfWork UOW;
        private readonly IHashService _hashService;

        public AuthService(IUnitOfWork uow, IHashService hashService)
        {
            UOW = uow;
            _hashService = hashService;
        }

        private async Task VerifyLoginIsUsed(string login, CancellationToken cancellationToken = default)
        {
            var auth = await UOW.Auths.GetByLogin(login, cancellationToken);
            if (auth is not null)
            {
                if (!auth.Active || !auth.RegisterActivated)
                {
                    throw new ValidationException(ValidationError.LoginInactiveUserExists.ToString());
                }
                else
                {
                    throw new ValidationException(ValidationError.LoginInUse.ToString());
                }
            }
        }

        private async Task VerifyEmailIsUsed(string email, CancellationToken cancellationToken = default)
        {
            var emailInUse = await UOW.Auths.IsEmailInUse(email, cancellationToken);
            if (emailInUse)
            {
                throw new ValidationException(ValidationError.EmailInUse.ToString());
            }
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
                throw new ValidationException(ValidationError.PasswordsDoNotMatch.ToString());
            }

            await VerifyLoginIsUsed(dto.Login, cancellationToken);
            await VerifyEmailIsUsed(dto.Email, cancellationToken);

            var user = new User()
            {
                Id = registerGuid,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            UOW.Users.Create(user);

            var auth = new Auth()
            {
                Id = registerGuid,
                Email = dto.Email,
                Login = dto.Login,
                Password = _hashService.HashPassword(dto.Password),
                AuthRole = AuthRole.User,
                RegisterGuid = Guid.NewGuid(),
                RegisterActivated = false,
                Active = false,
            };
            UOW.Auths.Create(auth);
            await UOW.SaveAsync(CancellationToken.None);
            // TODO: fill basic props like datecreated etc.
            // TODO: SEND ACTIVATION EMAIL
        }
        public Task<TokenDTO> RefreshSessionAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task ActivateUserAsync(Guid registerGuid, CancellationToken cancellationToken = default)
        {
            if (registerGuid == Guid.Empty)
            {
                throw new ValidationException(ValidationError.InvalidRegisterGuid.ToString());
            }
            var auth = await UOW.Auths.GetByRegisterGuid(registerGuid, cancellationToken);
            if (auth is null)
            {
                throw new ValidationException(ValidationError.ActivateUserFailed.ToString());
            } else if (auth.RegisterActivated)
            {
                throw new ValidationException(ValidationError.UserAlreadyActivated.ToString());
            } /* else if activation expired */

            auth.Active = true;
            auth.RegisterActivated = true;
            auth.RegisterGuid = null;
            await UOW.SaveAsync(cancellationToken);

            // TODO: SEND ACTIVATION CONFIRMATION EMAIL

        }
    }
}
