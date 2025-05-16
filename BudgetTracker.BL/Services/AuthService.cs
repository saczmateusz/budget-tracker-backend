using BudgetTracker.BL.Services.Interfaces;
using BudgetTracker.Core.Domain;
using BudgetTracker.Core.Enums;
using BudgetTracker.DAL.DTOs.Auth;
using BudgetTracker.DAL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationMinutes;
        private readonly int _registerAuthActivityHours;

        protected readonly IUnitOfWork UOW;
        private readonly IHashService _hashService;

        public AuthService(IUnitOfWork uow, IConfiguration configuration, IHashService hashService)
        {
            UOW = uow;
            _hashService = hashService;
            _accessTokenExpirationMinutes = int.Parse(configuration.GetSection("Auth:AccessTokenExpirationMinutes").Value!);
            _refreshTokenExpirationMinutes = int.Parse(configuration.GetSection("Auth:RefreshTokenExpirationMinutes").Value!);
            _registerAuthActivityHours = int.Parse(configuration.GetSection("Auth:RegisterTokenActivityHours").Value!);
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

        public async Task<TokenDTO> LoginAsync(LoginDTO dto, CancellationToken cancellationToken = default)
        {
            Auth? auth = null;
            try
            {
                if (dto.Login.Contains('@'))
                {
                    auth = await UOW.Auths.GetByEmail(dto.Login, cancellationToken);
                }
                else
                {
                    auth = await UOW.Auths.GetByLogin(dto.Login, cancellationToken);
                }
                if (auth is null)
                {
                    throw new Exception();
                }
                if (!auth.Active)
                {
                    throw new Exception();
                }
                if (!_hashService.VerifyPassword(dto.Password, auth.Password))
                {
                    throw new Exception();
                }

                var refreshToken = await UOW.RefreshTokens.GetUserLatestRefreshToken(auth.Id, cancellationToken);

                var tokenString = _hashService.GenerateToken(auth, _accessTokenExpirationMinutes);
                if (refreshToken is null)
                {
                    refreshToken = await CreateRefreshToken(auth.Id, cancellationToken);
                }
                else
                {
                    await ResetRefreshTokenExpirationTime(refreshToken, cancellationToken);
                }

                var result = new TokenDTO()
                {
                    AccessToken = tokenString,
                    RefreshToken = _hashService.SignRefreshToken(refreshToken),
                };
                return result;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
        }
        private async Task<RefreshToken> CreateRefreshToken(Guid authId, CancellationToken cancellationToken = default)
        {
            var refreshToken = _hashService.GenerateRefreshToken(authId, _refreshTokenExpirationMinutes);
            UOW.RefreshTokens.Create(refreshToken);
            await UOW.SaveAsync(cancellationToken);
            return refreshToken;
        }
        private async Task ResetRefreshTokenExpirationTime(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            UOW.RefreshTokens.Attach(refreshToken);
            refreshToken.Expiration = DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes);
            await UOW.SaveAsync(cancellationToken);
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
            } else if ((DateTime.UtcNow - auth.DateModified).TotalHours >= _registerAuthActivityHours)
            {
                throw new ValidationException(ValidationError.ActivateUserExpired.ToString());
            }

            auth.Active = true;
            auth.RegisterActivated = true;
            auth.RegisterGuid = null;
            await UOW.SaveAsync(cancellationToken);

            // TODO: SEND ACTIVATION CONFIRMATION EMAIL

        }
    }
}
