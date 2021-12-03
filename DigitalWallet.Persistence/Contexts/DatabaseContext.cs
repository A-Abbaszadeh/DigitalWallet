using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Persistence.Contexts
{
    public class DatabaseContext:DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a => a.UserId);
            modelBuilder.Entity<User>().HasOne(u => u.Account).WithOne(a => a.User).HasForeignKey<Account>(a => a.UserId);
            // ساخت شماره حساب به صورت خودکار و با حداقل 10 رقم
            modelBuilder.Entity<Account>(a =>
            {
                a.Property(propb => propb.AccountNumber).UseIdentityColumn(1000000000, 1);
            });

            modelBuilder.Entity<User>().HasIndex(u => u.NationalCode).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.MobileNumber).IsUnique();

            modelBuilder.Entity<Transaction>().Property(t => t.Debtor).HasDefaultValue(0);
            modelBuilder.Entity<Transaction>().Property(t => t.Creditor).HasDefaultValue(0);
        }
    }
}
