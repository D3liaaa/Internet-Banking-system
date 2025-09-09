
using Internet_Banking_System.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    
        public class InternetBankingContext : IdentityDbContext<User>
        {
            public InternetBankingContext(DbContextOptions<InternetBankingContext> options)
                : base(options)
            {
            }

            public DbSet<Account> Accounts { get; set; }
            public DbSet<Transaction> Transactions { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                });
            modelBuilder.Entity<IdentityRole>(entity =>
                {
                    entity.ToTable("Roles");
                });

                // Account Number Unique
                modelBuilder.Entity<Account>()
                            .HasIndex(a => a.AccountNumber)
                            .IsUnique();

                modelBuilder.Entity<Account>()
                            .HasOne(a => a.User)
                            .WithMany(u => u.Accounts)
                            .HasForeignKey(a => a.UserId);

                modelBuilder.Entity<Transaction>()
                            .HasOne(t => t.Account)
                            .WithMany(a => a.Transactions)
                            .HasForeignKey(t => t.AccountId);

                modelBuilder.Entity<Transaction>()
                            .HasOne(t => t.TargetAccount)
                            .WithMany()
                            .HasForeignKey(t => t.TargetAccountId)
                            .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
        }
    


