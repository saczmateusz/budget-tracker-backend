using BudgetTracker.Core.Enums;
using BudgetTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Core.Domain
{
    public class Auth : IDomainEntity
    {
        // IDomainEntity properties
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        // ---------------------------------------

        [Required]
        [MaxLength(1023)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(1023)]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public AuthRole AuthRole { get; set; }


        public DateTime? ResetPasswordRequestDate { get; set; }

        public Guid? ResetPasswordGuid { get; set; }

        public bool IsPasswordChanged { get; set; }

        public Guid? RegisterGuid { get; set; }

        public bool RegisterActivated { get; set; }


        public bool Active { get; set; }

        // ----------------------------------------------
        [Required]
        public User User { get; set; } = null!;
    }
}
