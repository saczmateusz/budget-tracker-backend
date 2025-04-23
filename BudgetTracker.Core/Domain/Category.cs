using BudgetTracker.Core.Enums;
using BudgetTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Core.Domain
{
    public class Category : IDomainEntity
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
        public string Name { get; set; } = string.Empty;
        [Required]
        public OperationType CategoryType { get; set; }
        [Required]
        public Guid UserId { get; set; }

        // ----------------------------------------------
        public User User { get; set; } = null!;
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
