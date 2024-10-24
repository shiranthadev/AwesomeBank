using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;

namespace AwesomeGIC.Bank.Persistence.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BankAccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(BankAccount bankAccount)
        {
            _dbContext.BankAccounts.Add(bankAccount);
        }

        public Task<BankAccount?> GetByBankAccountIdAsync(string bankAccountId)
        {
            return _dbContext.BankAccounts.FindAsync(bankAccountId).AsTask();
        }
    }
}
