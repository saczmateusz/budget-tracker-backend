using BudgetTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Core.Domain
{
    public class RefreshToken : IDomainEntity
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

        // ----------------------------------------------
        [Required]
        public DateTime Expiration { get; set; }

        // ----------------------------------------------
        [Required]
        public Guid UserId { get; set; }
    }
}
