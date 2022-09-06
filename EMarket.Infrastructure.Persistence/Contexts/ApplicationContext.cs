using EMarket.Core.Domain.Common;
using EMarket.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        DbSet<Advertisement> Advertisements { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultUserApp";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tables

            modelBuilder.Entity<Advertisement>()
                        .ToTable("Advertisements");

            modelBuilder.Entity<Category>()
                        .ToTable("Categories");

            modelBuilder.Entity<User>()
                        .ToTable("Users");

            #endregion

            #region "primary keys"

            modelBuilder.Entity<Advertisement>()
                        .HasKey(advertisement => advertisement.Id);

            modelBuilder.Entity<Category>()
                        .HasKey(category => category.Id);

            modelBuilder.Entity<User>()
                        .HasKey(user => user.Id);

            #endregion

            #region relationships

            modelBuilder.Entity<Category>()
                        .HasMany<Advertisement>(category => category.Advertisements)
                        .WithOne(advertisement => advertisement.Category)
                        .HasForeignKey(advertisement => advertisement.CategoryId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                        .HasMany<Advertisement>(category => category.Advertisements)
                        .WithOne(advertisement => advertisement.User)
                        .HasForeignKey(advertisement => advertisement.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region "propeties configuration"

            #region Advertisements

            modelBuilder.Entity<Advertisement>()
                        .Property(advertisement => advertisement.Name)
                        .HasMaxLength(100)
                        .IsRequired();

            modelBuilder.Entity<Advertisement>()
                        .Property(advertisement => advertisement.Description)
                        .IsRequired();

            modelBuilder.Entity<Advertisement>()
                        .Property(advertisement => advertisement.Price)
                        .IsRequired();

            #endregion

            #region Categories

            modelBuilder.Entity<Category>()
                        .Property(category => category.Name)
                        .HasMaxLength(100)
                        .IsRequired();

            modelBuilder.Entity<Category>()
                        .Property(category => category.Description)
                        .IsRequired();

            #endregion

            #region Users

            modelBuilder.Entity<User>()
                        .Property(user => user.FirstName)
                        .HasMaxLength(50)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(user => user.LastName)
                        .HasMaxLength(50)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(user => user.Username)
                        .HasMaxLength(100)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(user => user.Email)
                        .HasMaxLength(100)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(user => user.Phone)
                        .HasMaxLength(30)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(user => user.Password)
                        .IsRequired();

            modelBuilder.Entity<Advertisement>()
                        .Property(advertisement => advertisement.Description)
                        .IsRequired();

            #endregion

            #endregion
        }
    }
}
