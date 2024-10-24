using AwesomeGIC.Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGIC.Bank.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<AccountTxn> AccountTxns { get; set; }

        public DbSet<InterestRule> InterestRules { get; set; }
    }
}
