using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.DAL.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
