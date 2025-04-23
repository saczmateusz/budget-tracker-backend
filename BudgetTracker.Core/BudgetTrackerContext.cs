using BudgetTracker.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Core
{
    public class BudgetTrackerContext : DbContext
    {
        private const string _keyIsDeleted = "IsDeleted";
        private const string _keyDbFnNEWID = "NEWID()";

        public BudgetTrackerContext(DbContextOptions<BudgetTrackerContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Auth> Auths { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Operation> Operations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // obsługa globalna typu decimal przy zapisie do bazy - 20 cyfr, 2 miejsca po przecinku
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(decimal) || x.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(20,2)");
            }


            builder.Entity<Auth>().Property<bool>(_keyIsDeleted);
            builder.Entity<Auth>().HasQueryFilter(x => EF.Property<bool>(x, _keyIsDeleted) == false);
            builder.Entity<Auth>().Property(x => x.AuthRole).HasConversion<string>();


            builder.Entity<User>().Property<bool>(_keyIsDeleted);
            builder.Entity<User>().HasQueryFilter(x => EF.Property<bool>(x, _keyIsDeleted) == false);
            //builder.Entity<User>().Property(x => x.Role).HasConversion<string>();
            // Relacja one to zero - jednostronna 1:1
            builder.Entity<User>()
                .HasOne(x => x.Auth)
                .WithOne(y => y.User)
                .HasForeignKey<Auth>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Category>().Property<bool>(_keyIsDeleted);
            builder.Entity<Category>().HasQueryFilter(x => EF.Property<bool>(x, _keyIsDeleted) == false);
            builder.Entity<Category>().Property(x => x.CategoryType).HasConversion<string>();
            // Relacja one to many
            builder.Entity<Category>()
                .HasOne(x => x.User)
                .WithMany(y => y.Categories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction); // TODO: cascade?


            builder.Entity<Operation>().Property<bool>(_keyIsDeleted);
            builder.Entity<Operation>().HasQueryFilter(x => EF.Property<bool>(x, _keyIsDeleted) == false);
            builder.Entity<Operation>().Property(x => x.OperationType).HasConversion<string>();
            // Relacja one to many
            builder.Entity<Operation>()
                .HasOne(x => x.User)
                .WithMany(y => y.Operations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction); // TODO: cascade?
            // Relacja one to one
            builder.Entity<Operation>()
                .HasOne(x => x.Category)
                .WithMany(y => y.Operations)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); // TODO: cascade?
        }

        // TODO: override save methods - update properties like "created by" etc. by default
    }
}
