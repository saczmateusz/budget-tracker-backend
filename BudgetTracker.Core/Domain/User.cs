using BudgetTracker.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Core.Domain
{
    public class User : IDomainEntity
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
        public Auth Auth { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;


        // ---------------------------------------
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
