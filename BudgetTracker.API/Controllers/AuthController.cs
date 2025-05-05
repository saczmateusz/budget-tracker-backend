using BudgetTracker.BL.Interfaces;
using BudgetTracker.DAL.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(dto, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto, CancellationToken cancellationToken = default)
        {
            await _authService.RegisterAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("Refresh/{refreshToken}")]
        public async Task<IActionResult> Refresh(string refreshToken, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RefreshSessionAsync(refreshToken, cancellationToken);
            return Ok(result);
        }

        // Logout
        // Ping?
        // ResetPassword
        // ...

        [HttpPost]
        [Route("Activate/{registerGuid}")]
        public async Task<IActionResult> Activate(Guid registerGuid, CancellationToken cancellationToken = default)
        {
            await _authService.ActivateUserAsync(registerGuid, cancellationToken);
            return Ok();
        }
    }
}
