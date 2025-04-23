using BudgetTracker.Core.Enums;
using BudgetTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Core.Domain
{
    public class Operation : IDomainEntity
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
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public Currency Currency { get; set; }

        // ---------------------------------------
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Category Category { get; set; } = null!;
        [Required]
        public OperationType OperationType { get; set; }
        [Required]
        public Guid UserId { get; set; }

        // ----------------------------------------------
        public User User { get; set; } = null!;

    }
}
