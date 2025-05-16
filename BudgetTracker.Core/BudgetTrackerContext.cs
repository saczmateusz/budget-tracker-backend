using BudgetTracker.Core.Domain;
using BudgetTracker.Core.Interfaces;
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
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
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


            builder.Entity<RefreshToken>().Property<bool>(_keyIsDeleted);
            builder.Entity<RefreshToken>().HasQueryFilter(x => EF.Property<bool>(x, _keyIsDeleted) == false);
            builder.Entity<RefreshToken>().HasIndex(x => x.UserId);


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

        // ----------------------------------------------

        public override int SaveChanges()
        {
            UpdateCoreProperties();
            return base.SaveChanges();
        }
        public int SaveChangesNoSideEffect()
        {
            UpdateCoreProperties(true);
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateCoreProperties();
            return base.SaveChangesAsync(cancellationToken);
        }
        public Task<int> SaveChangesNoSideEffectAsync(CancellationToken cancellationToken = default)
        {
            UpdateCoreProperties(true);
            return base.SaveChangesAsync(cancellationToken);
        }

        // TODO: override save methods - update properties like "created by" etc. by default
        private void UpdateCoreProperties(bool noSideEffect = false)
        {
            var allTrackedEntities = ChangeTracker.Entries().ToList();
            var trackedEntities = allTrackedEntities.Where(x => x.State == EntityState.Added ||
                                                                x.State == EntityState.Modified ||
                                                                x.State == EntityState.Deleted)
                                                    .ToList();
            foreach (var entry in trackedEntities)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (!noSideEffect)
                        {
                            entry.CurrentValues[nameof(IDomainEntity.IsDeleted)] = false;
                            entry.CurrentValues[nameof(IDomainEntity.DateCreated)] = DateTime.UtcNow;
                            entry.CurrentValues[nameof(IDomainEntity.DateModified)] = DateTime.UtcNow;
                        }
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        if (!noSideEffect)
                        {
                            entry.CurrentValues[nameof(IDomainEntity.DateModified)] = DateTime.UtcNow;
                        }
                        break;
                }
            }
        }
    }
}
