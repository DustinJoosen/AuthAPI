using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.API.Core.Entities;

namespace Auth.API.Persistence
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.UserName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entity in this.ChangeTracker.Entries())
            {
                if (entity.Entity is not AuditableEntity auditableEntity)
                {
                    continue;
                }

                if (entity.State == EntityState.Added)
                {
                    auditableEntity.CreatedOn = DateTime.UtcNow;
                }

                if (entity.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedOn = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in this.ChangeTracker.Entries())
            {
                if (entity.Entity is not AuditableEntity auditableEntity)
                {
                    continue;
                }

                if (entity.State == EntityState.Added)
                {
                    auditableEntity.CreatedOn = DateTime.UtcNow;
                }

                if (entity.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedOn = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
